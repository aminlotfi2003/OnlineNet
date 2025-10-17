using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineNet.Application.Baskets.Dtos;
using OnlineNet.Application.Baskets.Queries.GetCustomerBasket;
using OnlineNet.Application.Customers.Dtos;
using OnlineNet.Application.Customers.Queries.GetCustomer;
using OnlineNet.Application.Orders.Dtos;
using OnlineNet.Application.Orders.Queries.GetCustomerOrders;

namespace OnlineNet.WebApp.Pages.Customers;

public class DetailsModel : PageModel
{
    private readonly IMediator _mediator;

    public DetailsModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public CustomerDetailDto? Customer { get; private set; }
    public BasketDto? Basket { get; private set; }
    public List<OrderSummaryDto> Orders { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
    {
        Customer = await _mediator.Send(new GetCustomerQuery(id), cancellationToken);
        if (Customer is null)
        {
            return NotFound();
        }

        Basket = await _mediator.Send(new GetCustomerBasketQuery(id), cancellationToken);
        Orders = await _mediator.Send(new GetCustomerOrdersQuery(id), cancellationToken);

        return Page();
    }
}
