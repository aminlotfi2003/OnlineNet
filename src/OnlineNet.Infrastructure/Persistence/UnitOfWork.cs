using Microsoft.EntityFrameworkCore.Storage;
using OnlineNet.Application.Common.Abstractions;

namespace OnlineNet.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    private IDbContextTransaction? _currentTx;

    public UnitOfWork(ApplicationDbContext db) => _db = db;

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);

    public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken ct = default)
    {
        if (_currentTx is not null)
        {
            // Nested call: reuse same transaction
            await action(ct);
            return;
        }

        await using var tx = _currentTx = await _db.Database.BeginTransactionAsync(ct);
        try
        {
            await action(ct);
            await _db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
        finally
        {
            await tx.DisposeAsync();
            _currentTx = null;
        }
    }

    public async Task<T> ExecuteInTransactionAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken ct = default)
    {
        T result = default!;
        await ExecuteInTransactionAsync(async c =>
        {
            result = await action(c);
        }, ct);
        return result!;
    }

    public async ValueTask DisposeAsync()
    {
        if (_currentTx is not null)
        {
            await _currentTx.DisposeAsync();
            _currentTx = null;
        }
    }
}
