using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineNet.Application.Orders.Dtos;
using OnlineNet.Application.Orders.Queries.ListRecentOrders;

namespace OnlineNet.WebApp.Pages.Orders;

public class IndexModel : PageModel
{
    private readonly IMediator _mediator;

    public IndexModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [BindProperty(SupportsGet = true)]
    public int? Limit { get; set; }

    public List<OrderListItemDto> Orders { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var limit = Limit.GetValueOrDefault(ListRecentOrdersQuery.DefaultLimit);
        Orders = await _mediator.Send(new ListRecentOrdersQuery(limit), cancellationToken);
    }
}
