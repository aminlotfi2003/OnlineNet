using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineNet.Application.Products.Commands.UpdateProduct;
using OnlineNet.Application.Products.Queries.GetProductById;

namespace OnlineNet.WebApp.Pages.Products;

public class EditModel : PageModel
{
    private readonly IMediator _mediator;
    public EditModel(IMediator mediator) => _mediator = mediator;

    [BindProperty]
    public UpdateProductCommand Command { get; set; } = new(Guid.Empty, "", "", 0, "USD", null, 0, true);

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken ct)
    {
        var p = await _mediator.Send(new GetProductByIdQuery(id), ct);

        Command = new UpdateProductCommand(
            p.Id, p.Name, p.Sku, p.PriceAmount, p.PriceCurrency,
            p.Description, p.StockQuantity, p.IsActive);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid) return Page();

        await _mediator.Send(Command, ct);
        TempData["SuccessMessage"] = "Product updated.";
        return RedirectToPage("Index");
    }
}
