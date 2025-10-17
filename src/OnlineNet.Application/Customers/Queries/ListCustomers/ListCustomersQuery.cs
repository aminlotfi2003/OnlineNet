using OnlineNet.Application.Common.CQRS;
using OnlineNet.Application.Customers.Dtos;

namespace OnlineNet.Application.Customers.Queries.ListCustomers;

public sealed record ListCustomersQuery() : IQuery<List<CustomerSummaryDto>>;
