namespace OnlineNet.Application.Products.Dtos;

public sealed record ProductDto(
    Guid Id,
    string Name,
    string Sku,
    decimal PriceAmount,
    string PriceCurrency,
    int StockQuantity,
    bool IsActive,
    string? Description
);
