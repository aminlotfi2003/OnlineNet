namespace OnlineNet.Application.Orders.Dtos;

public sealed record OrderLineDto(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, string Currency)
{
    public decimal Subtotal => decimal.Round(UnitPrice * Quantity, 2);
}
