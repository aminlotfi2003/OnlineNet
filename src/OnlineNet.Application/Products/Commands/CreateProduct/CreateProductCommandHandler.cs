using MediatR;
using OnlineNet.Application.Common.Abstractions;
using OnlineNet.Application.Common.Exceptions;
using OnlineNet.Domain.Products;
using OnlineNet.Domain.Products.ValueObjects;

namespace OnlineNet.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateProductCommandHandler(IProductRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken ct)
    {
        if (await _repo.ExistsBySkuAsync(request.Sku, ct))
            throw new ConflictException($"SKU '{request.Sku}' already exists.");

        var product = new Product(
            name: request.Name,
            sku: new Sku(request.Sku),
            price: new Money(request.PriceAmount, request.PriceCurrency),
            description: request.Description,
            stockQuantity: request.StockQuantity
        );

        await _repo.AddAsync(product, ct);

        await _uow.SaveChangesAsync(ct);

        return product.Id;
    }
}
