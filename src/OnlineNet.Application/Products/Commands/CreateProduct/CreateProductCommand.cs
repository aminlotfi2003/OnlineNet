using OnlineNet.Application.Common.CQRS;

namespace OnlineNet.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string Sku,
    decimal PriceAmount,
    string PriceCurrency,
    string? Description,
    int StockQuantity
) : ICommand<Guid>;
