namespace OnlineNet.Application.Orders.Dtos;

public sealed record PlaceOrderItemDto(Guid ProductId, int Quantity);
