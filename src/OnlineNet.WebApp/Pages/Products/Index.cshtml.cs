using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineNet.Application.Products.Dtos;
using OnlineNet.Application.Products.Queries.ListProducts;

namespace OnlineNet.WebApp.Pages.Products;

public class IndexModel : PageModel
{
    private readonly IMediator _mediator;
    public IndexModel(IMediator mediator) => _mediator = mediator;

    public List<ProductDto> Products { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        Products = await _mediator.Send(new ListProductsQuery(), ct);
    }

    public string? SuccessMessage
    {
        get => TempData[nameof(SuccessMessage)] as string;
        set => TempData[nameof(SuccessMessage)] = value;
    }
}
