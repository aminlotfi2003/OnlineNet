using MediatR;
using OnlineNet.Application.Customers.Dtos;
using OnlineNet.Domain.Customers;

namespace OnlineNet.Application.Customers.Queries.ListCustomers;

public sealed class ListCustomersQueryHandler : IRequestHandler<ListCustomersQuery, List<CustomerSummaryDto>>
{
    private readonly ICustomerRepository _customerRepository;

    public ListCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<CustomerSummaryDto>> Handle(ListCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.ListAsync(cancellationToken);

        return customers
            .Select(c => new CustomerSummaryDto(
                c.Id,
                c.Name.FullName,
                c.Email.Value))
            .ToList();
    }
}
