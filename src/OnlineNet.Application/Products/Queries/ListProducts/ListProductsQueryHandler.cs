using MediatR;
using OnlineNet.Application.Products.Dtos;
using OnlineNet.Domain.Catalog.Products;

namespace OnlineNet.Application.Products.Queries.ListProducts;

public sealed class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _repo;

    public ListProductsQueryHandler(IProductRepository repo) => _repo = repo;

    public async Task<List<ProductDto>> Handle(ListProductsQuery request, CancellationToken ct)
    {
        var items = await _repo.ListAsync(ct);

        return items.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Sku.Value,
            p.Price.Amount,
            p.Price.Currency,
            p.StockQuantity,
            p.IsActive,
            p.Description
        )).ToList();
    }
}
