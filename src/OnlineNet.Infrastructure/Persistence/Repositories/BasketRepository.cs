using Microsoft.EntityFrameworkCore;
using OnlineNet.Domain.Baskets;

namespace OnlineNet.Infrastructure.Persistence.Repositories;

public sealed class BasketRepository(ApplicationDbContext db) : IBasketRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<Basket?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken ct = default)
    {
        var query = _db.Baskets
            .Include(b => b.Items)
            .AsQueryable();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(b => b.Id == id, ct);
    }

    public async Task<Basket?> GetByCustomerIdAsync(Guid customerId, bool asNoTracking = true, CancellationToken ct = default)
    {
        var query = _db.Baskets
            .Include(b => b.Items)
            .Where(b => b.CustomerId == customerId)
            .AsQueryable();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(ct);
    }

    public Task AddAsync(Basket basket, CancellationToken ct = default)
        => _db.Baskets.AddAsync(basket, ct).AsTask();

    public void Update(Basket basket) => _db.Baskets.Update(basket);
}
