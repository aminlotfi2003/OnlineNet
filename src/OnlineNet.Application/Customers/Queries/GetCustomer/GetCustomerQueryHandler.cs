using MediatR;
using OnlineNet.Application.Customers.Dtos;
using OnlineNet.Domain.Customers;

namespace OnlineNet.Application.Customers.Queries.GetCustomer;

public sealed class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDetailDto?>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDetailDto?> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, ct: cancellationToken);
        if (customer is null)
        {
            return null;
        }

        return new CustomerDetailDto(
            customer.Id,
            customer.Name.FullName,
            customer.Email.Value,
            customer.ActiveBasketId,
            customer.CreatedOn,
            customer.ModifiedOn);
    }
}
