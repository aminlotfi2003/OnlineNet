using Microsoft.EntityFrameworkCore;
using OnlineNet.Domain.Customers;

namespace OnlineNet.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository(ApplicationDbContext db) : ICustomerRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<Customer?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken ct = default)
    {
        var query = _db.Customers.AsQueryable();
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        var normalized = email.Trim().ToLowerInvariant();
        return await _db.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email.Value == normalized, ct);
    }

    public Task AddAsync(Customer customer, CancellationToken ct = default)
        => _db.Customers.AddAsync(customer, ct).AsTask();

    public void Update(Customer customer) => _db.Customers.Update(customer);

    public async Task<List<Customer>> ListAsync(CancellationToken ct = default)
    {
        return await _db.Customers
            .AsNoTracking()
            .OrderBy(c => c.Name.FirstName)
            .ThenBy(c => c.Name.LastName)
            .ToListAsync(ct);
    }

    public async Task<List<Customer>> ListByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        if (ids is null)
        {
            return [];
        }

        var distinctIds = ids
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        if (distinctIds.Count == 0)
        {
            return [];
        }

        return await _db.Customers
            .AsNoTracking()
            .Where(c => distinctIds.Contains(c.Id))
            .ToListAsync(ct);
    }
}
