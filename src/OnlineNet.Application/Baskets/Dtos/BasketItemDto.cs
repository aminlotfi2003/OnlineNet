namespace OnlineNet.Application.Baskets.Dtos;

public sealed record BasketItemDto(Guid ProductId, string ProductName, decimal UnitPrice, string Currency, int Quantity)
{
    public decimal Subtotal => decimal.Round(UnitPrice * Quantity, 2);
}
