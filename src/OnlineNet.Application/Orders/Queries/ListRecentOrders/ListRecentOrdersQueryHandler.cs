using MediatR;
using OnlineNet.Application.Orders.Dtos;
using OnlineNet.Domain.Customers;
using OnlineNet.Domain.Orders;

namespace OnlineNet.Application.Orders.Queries.ListRecentOrders;

public sealed class ListRecentOrdersQueryHandler : IRequestHandler<ListRecentOrdersQuery, List<OrderListItemDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;

    public ListRecentOrdersQueryHandler(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }

    public async Task<List<OrderListItemDto>> Handle(ListRecentOrdersQuery request, CancellationToken cancellationToken)
    {
        var limit = request.Limit <= 0 ? ListRecentOrdersQuery.DefaultLimit : request.Limit;
        var orders = await _orderRepository.ListRecentAsync(limit, cancellationToken);

        if (orders.Count == 0)
        {
            return [];
        }

        var customerIds = orders
            .Select(order => order.CustomerId)
            .Distinct()
            .ToList();

        var customers = await _customerRepository.ListByIdsAsync(customerIds, cancellationToken);
        var customerLookup = customers.ToDictionary(c => c.Id, c => c.Name.FullName);

        return orders
            .Select(order => new OrderListItemDto(
                order.Id,
                order.CustomerId,
                customerLookup.GetValueOrDefault(order.CustomerId, "Unknown customer"),
                order.Status.ToString(),
                order.Total.Amount,
                order.Total.Currency,
                order.CreatedOn,
                order.Lines.Count))
            .ToList();
    }
}
