namespace OnlineNet.Domain.Products;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken ct = default);
    Task<Product?> GetBySkuAsync(string sku, CancellationToken ct = default);
    Task<bool> ExistsBySkuAsync(string sku, CancellationToken ct = default);
    Task<List<Product>> ListAsync(CancellationToken ct = default);
    Task AddAsync(Product product, CancellationToken ct = default);
    void Update(Product product);
    void Remove(Product product);
}
