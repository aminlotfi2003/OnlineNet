using MediatR;
using OnlineNet.Application.Orders.Dtos;
using OnlineNet.Domain.Orders;

namespace OnlineNet.Application.Orders.Queries.GetCustomerOrders;

public sealed class GetCustomerOrdersQueryHandler : IRequestHandler<GetCustomerOrdersQuery, List<OrderSummaryDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetCustomerOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderSummaryDto>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.ListByCustomerIdAsync(request.CustomerId, cancellationToken);

        return orders
            .Select(order => new OrderSummaryDto(
                order.Id,
                order.CustomerId,
                order.Status.ToString(),
                order.Total.Amount,
                order.Total.Currency,
                order.CreatedOn,
                order.Lines
                    .Select(line => new OrderLineDto(
                        line.ProductId,
                        line.ProductName,
                        line.Quantity,
                        line.UnitPrice.Amount,
                        line.UnitPrice.Currency))
                    .ToList()))
            .ToList();
    }
}
