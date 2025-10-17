using MediatR;
using OnlineNet.Application.Customers.Dtos;

namespace OnlineNet.Application.Customers.Queries.GetCustomer;

public sealed record GetCustomerQuery(Guid CustomerId) : IRequest<CustomerDetailDto?>;
