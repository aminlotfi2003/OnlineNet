using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineNet.Application.Customers.Dtos;
using OnlineNet.Application.Customers.Queries.ListCustomers;

namespace OnlineNet.WebApp.Pages.Customers;

public class IndexModel : PageModel
{
    private readonly IMediator _mediator;

    public IndexModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public List<CustomerSummaryDto> Customers { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Customers = await _mediator.Send(new ListCustomersQuery(), cancellationToken);
    }
}
