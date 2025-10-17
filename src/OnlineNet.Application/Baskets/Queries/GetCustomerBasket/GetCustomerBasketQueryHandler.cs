using MediatR;
using OnlineNet.Application.Baskets.Dtos;
using OnlineNet.Domain.Baskets;

namespace OnlineNet.Application.Baskets.Queries.GetCustomerBasket;

public sealed class GetCustomerBasketQueryHandler : IRequestHandler<GetCustomerBasketQuery, BasketDto?>
{
    private readonly IBasketRepository _basketRepository;

    public GetCustomerBasketQueryHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<BasketDto?> Handle(GetCustomerBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetByCustomerIdAsync(request.CustomerId, ct: cancellationToken);

        if (basket is null)
        {
            return null;
        }

        var items = basket.Items
            .Select(i => new BasketItemDto(
                i.ProductId,
                i.ProductName,
                i.UnitPrice.Amount,
                i.UnitPrice.Currency,
                i.Quantity))
            .ToList();

        return new BasketDto(basket.Id, basket.CustomerId, items);
    }
}
