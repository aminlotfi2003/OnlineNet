using MediatR;
using OnlineNet.Application.Common.CQRS;

namespace OnlineNet.Application.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Sku,
    decimal PriceAmount,
    string PriceCurrency,
    string? Description,
    int StockQuantity,
    bool IsActive
) : ICommand<Unit>;
