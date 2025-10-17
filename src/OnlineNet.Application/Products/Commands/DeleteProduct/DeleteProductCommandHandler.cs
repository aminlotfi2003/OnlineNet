using MediatR;
using OnlineNet.Application.Common.Abstractions;
using OnlineNet.Application.Common.Exceptions;
using OnlineNet.Domain.Products;

namespace OnlineNet.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteProductCommandHandler(IProductRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var p = await _repo.GetByIdAsync(request.Id, asNoTracking: false, ct)
                ?? throw new NotFoundException("Product", request.Id);

        _repo.Remove(p);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
