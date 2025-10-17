using MediatR;
using OnlineNet.Application.Common.Abstractions;
using OnlineNet.Application.Common.Exceptions;
using OnlineNet.Application.Orders.Dtos;
using OnlineNet.Domain.Baskets;
using OnlineNet.Domain.Customers;
using OnlineNet.Domain.Orders;
using OnlineNet.Domain.Orders.ValueObjects;
using OnlineNet.Domain.Products;

namespace OnlineNet.Application.Orders.Commands.PlaceOrder;

public sealed class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, PlaceOrderResultDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PlaceOrderCommandHandler(
        ICustomerRepository customerRepository,
        IOrderRepository orderRepository,
        IBasketRepository basketRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
        _basketRepository = basketRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PlaceOrderResultDto> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, asNoTracking: false, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        return await _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var orderLines = new List<OrderLine>(request.Items.Count);

            var basket = await _basketRepository.GetByCustomerIdAsync(request.CustomerId, asNoTracking: false, ct);
            var basketWasCreated = false;
            if (basket is null)
            {
                basket = new Basket(request.CustomerId);
                await _basketRepository.AddAsync(basket, ct);
                basketWasCreated = true;
            }

            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId, asNoTracking: false, ct)
                    ?? throw new NotFoundException(nameof(Product), item.ProductId);

                if (!product.IsActive)
                    throw new ConflictException($"Product '{product.Name}' is not available for ordering.");

                if (product.StockQuantity < item.Quantity)
                {
                    throw new ConflictException(
                        $"Insufficient stock for product '{product.Name}'. Requested {item.Quantity}, available {product.StockQuantity}.");
                }

                product.DecreaseStock(item.Quantity);
                _productRepository.Update(product);

                var orderLine = new OrderLine(product.Id, product.Name, item.Quantity, product.Price);
                orderLines.Add(orderLine);

                basket.AddOrUpdateItem(product.Id, product.Name, product.Price, item.Quantity);
            }

            if (basketWasCreated || customer.ActiveBasketId != basket.Id)
            {
                customer.AssignBasket(basket.Id);
                _customerRepository.Update(customer);
            }

            if (!basketWasCreated)
            {
                _basketRepository.Update(basket);
            }

            var order = Order.Create(customer.Id, orderLines);
            await _orderRepository.AddAsync(order, ct);

            return new PlaceOrderResultDto(order.Id);
        }, cancellationToken);
    }
}
