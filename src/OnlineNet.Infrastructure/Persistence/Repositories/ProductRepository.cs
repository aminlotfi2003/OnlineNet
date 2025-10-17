using Microsoft.EntityFrameworkCore;
using OnlineNet.Domain.Products;

namespace OnlineNet.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository(ApplicationDbContext db) : IProductRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<Product?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken ct = default)
    {
        var query = _db.Products.AsQueryable();
        if (asNoTracking) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(sku)) return null;

        return await _db.Products
            .AsNoTracking()
            .Where(p => p.Sku.Value == sku.ToUpper())
            .FirstOrDefaultAsync(ct);
    }

    public Task<bool> ExistsBySkuAsync(string sku, CancellationToken ct = default)
        => _db.Products.AsNoTracking().AnyAsync(p => p.Sku.Value == sku.ToUpper(), ct);

    public async Task<List<Product>> ListAsync(CancellationToken ct = default)
        => await _db.Products.AsNoTracking()
            .OrderByDescending(p => p.CreatedOn)
            .ToListAsync(ct);

    public Task AddAsync(Product product, CancellationToken ct = default)
        => _db.Products.AddAsync(product, ct).AsTask();

    public void Update(Product product) => _db.Products.Update(product);
    public void Remove(Product product) => _db.Products.Remove(product);
}
