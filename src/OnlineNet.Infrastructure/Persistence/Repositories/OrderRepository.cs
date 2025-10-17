using Microsoft.EntityFrameworkCore;
using OnlineNet.Domain.Orders;

namespace OnlineNet.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository(ApplicationDbContext db) : IOrderRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<Order?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken ct = default)
    {
        var query = _db.Orders
            .Include(o => o.Lines)
            .AsQueryable();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(o => o.Id == id, ct);
    }

    public Task AddAsync(Order order, CancellationToken ct = default)
        => _db.Orders.AddAsync(order, ct).AsTask();

    public void Update(Order order) => _db.Orders.Update(order);

    public async Task<List<Order>> ListByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
    {
        return await _db.Orders
            .Include(o => o.Lines)
            .AsNoTracking()
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedOn)
            .ToListAsync(ct);
    }

    public async Task<List<Order>> ListRecentAsync(int take, CancellationToken ct = default)
    {
        var limit = Math.Clamp(take, 1, 200);

        return await _db.Orders
            .Include(o => o.Lines)
            .AsNoTracking()
            .OrderByDescending(o => o.CreatedOn)
            .Take(limit)
            .ToListAsync(ct);
    }
}
