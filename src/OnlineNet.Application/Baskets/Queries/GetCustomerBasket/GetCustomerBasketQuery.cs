using OnlineNet.Application.Baskets.Dtos;
using OnlineNet.Application.Common.CQRS;

namespace OnlineNet.Application.Baskets.Queries.GetCustomerBasket;

public sealed record GetCustomerBasketQuery(Guid CustomerId) : IQuery<BasketDto?>;
