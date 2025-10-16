using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineNet.Application.Products.Dtos;
using OnlineNet.Application.Products.Queries.GetProductById;

namespace OnlineNet.WebApp.Pages.Products;

public class DetailsModel : PageModel
{
    private readonly IMediator _mediator;
    public DetailsModel(IMediator mediator) => _mediator = mediator;

    public ProductDto? Product { get; private set; }

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken ct)
    {
        Product = await _mediator.Send(new GetProductByIdQuery(id), ct);
        return Page();
    }
}
