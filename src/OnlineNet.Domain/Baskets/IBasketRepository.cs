namespace OnlineNet.Domain.Baskets;

public interface IBasketRepository
{
    Task<Basket?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken ct = default);
    Task<Basket?> GetByCustomerIdAsync(Guid customerId, bool asNoTracking = true, CancellationToken ct = default);
    Task AddAsync(Basket basket, CancellationToken ct = default);
    void Update(Basket basket);
}
