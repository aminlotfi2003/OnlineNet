using MediatR;
using OnlineNet.Application.Common.Abstractions;

namespace OnlineNet.Application.Common.Behaviors;

public sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly IUnitOfWork _uow;

    public TransactionBehavior(IUnitOfWork uow) => _uow = uow;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var isCommand = typeof(TRequest).Name.EndsWith("Command", StringComparison.OrdinalIgnoreCase);
        if (!isCommand) return await next();

        TResponse? result = default!;
        await _uow.ExecuteInTransactionAsync(async _ =>
        {
            result = await next();
            await _uow.SaveChangesAsync(ct);
        }, ct);

        return result!;
    }
}
