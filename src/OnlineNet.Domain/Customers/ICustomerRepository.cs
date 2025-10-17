namespace OnlineNet.Domain.Customers;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, bool asNoTracking = true, CancellationToken ct = default);
    Task<Customer?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<List<Customer>> ListAsync(CancellationToken ct = default);
    Task<List<Customer>> ListByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    Task AddAsync(Customer customer, CancellationToken ct = default);
    void Update(Customer customer);
}
