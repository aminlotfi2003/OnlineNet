using MediatR;
using OnlineNet.Application.Common.Abstractions;
using OnlineNet.Application.Common.Exceptions;
using OnlineNet.Domain.Products.ValueObjects;
using OnlineNet.Domain.Products;

namespace OnlineNet.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateProductCommandHandler(IProductRepository repo, IUnitOfWork uow)
    {
        _repo = repo; _uow = uow;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var p = await _repo.GetByIdAsync(request.Id, asNoTracking: false, ct)
                ?? throw new NotFoundException("Product", request.Id);

        if (!string.Equals(p.Sku.Value, request.Sku, StringComparison.OrdinalIgnoreCase))
        {
            var exists = await _repo.ExistsBySkuAsync(request.Sku, ct);
            if (exists) throw new ConflictException($"SKU '{request.Sku}' already exists.");
        }

        p.Rename(request.Name);
        p.ChangeSku(new Sku(request.Sku));
        p.ChangePrice(new Money(request.PriceAmount, request.PriceCurrency));
        p.UpdateDescription(request.Description);
        if (request.StockQuantity != p.StockQuantity)
        {
            if (request.StockQuantity > p.StockQuantity)
                p.IncreaseStock(request.StockQuantity - p.StockQuantity);
            else
                p.DecreaseStock(p.StockQuantity - request.StockQuantity);
        }
        if (request.IsActive && !p.IsActive) p.Activate();
        if (!request.IsActive && p.IsActive) p.Deactivate();

        _repo.Update(p);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
