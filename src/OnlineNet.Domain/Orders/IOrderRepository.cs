namespace OnlineNet.Domain.Orders;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken ct = default);
    Task<List<Order>> ListByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
    Task<List<Order>> ListRecentAsync(int take, CancellationToken ct = default);
    Task AddAsync(Order order, CancellationToken ct = default);
    void Update(Order order);
}
