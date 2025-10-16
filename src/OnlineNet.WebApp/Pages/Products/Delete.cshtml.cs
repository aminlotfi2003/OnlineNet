using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineNet.Application.Products.Commands.DeleteProduct;
using OnlineNet.Application.Products.Dtos;
using OnlineNet.Application.Products.Queries.GetProductById;

namespace OnlineNet.WebApp.Pages.Products;

public class DeleteModel : PageModel
{
    private readonly IMediator _mediator;
    public DeleteModel(IMediator mediator) => _mediator = mediator;

    public ProductDto? Product { get; private set; }

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken ct)
    {
        Product = await _mediator.Send(new GetProductByIdQuery(id), ct);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteProductCommand(id), ct);
        TempData["SuccessMessage"] = "Product deleted.";
        return RedirectToPage("Index");
    }
}
