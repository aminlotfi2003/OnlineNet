namespace OnlineNet.Application.Baskets.Dtos;

public sealed record BasketDto(Guid Id, Guid CustomerId, IReadOnlyCollection<BasketItemDto> Items)
{
    public bool HasItems => Items.Count > 0;

    public string Currency => Items.FirstOrDefault()?.Currency ?? "";

    public decimal Total => decimal.Round(Items.Sum(i => i.Subtotal), 2);
}
