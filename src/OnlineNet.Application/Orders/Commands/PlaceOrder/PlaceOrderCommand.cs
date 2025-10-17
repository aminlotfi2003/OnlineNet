using OnlineNet.Application.Common.CQRS;
using OnlineNet.Application.Orders.Dtos;

namespace OnlineNet.Application.Orders.Commands.PlaceOrder;

public sealed record PlaceOrderCommand(Guid CustomerId, IReadOnlyCollection<PlaceOrderItemDto> Items) : ICommand<PlaceOrderResultDto>;
