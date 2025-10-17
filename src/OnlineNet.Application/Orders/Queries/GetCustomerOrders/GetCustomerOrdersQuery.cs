using OnlineNet.Application.Common.CQRS;
using OnlineNet.Application.Orders.Dtos;

namespace OnlineNet.Application.Orders.Queries.GetCustomerOrders;

public sealed record GetCustomerOrdersQuery(Guid CustomerId) : IQuery<List<OrderSummaryDto>>;
