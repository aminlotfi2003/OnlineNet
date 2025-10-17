using MediatR;
using OnlineNet.Application.Orders.Dtos;

namespace OnlineNet.Application.Orders.Queries.ListRecentOrders;

public sealed record ListRecentOrdersQuery(int Limit = 50) : IRequest<List<OrderListItemDto>>
{
    public const int DefaultLimit = 50;
}
