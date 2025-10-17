using MediatR;
using OnlineNet.Application.Common.Exceptions;
using OnlineNet.Application.Products.Dtos;
using OnlineNet.Domain.Products;

namespace OnlineNet.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _repo;
    public GetProductByIdQueryHandler(IProductRepository repo) => _repo = repo;

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var p = await _repo.GetByIdAsync(request.Id, asNoTracking: true, ct)
                ?? throw new NotFoundException("Product", request.Id);

        return new ProductDto(p.Id, p.Name, p.Sku.Value, p.Price.Amount, p.Price.Currency,
                              p.StockQuantity, p.IsActive, p.Description);
    }
}
