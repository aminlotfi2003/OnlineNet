using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineNet.Application.Common.Exceptions;
using OnlineNet.Application.Products.Commands.CreateProduct;

namespace OnlineNet.WebApp.Pages.Products;

public class CreateModel : PageModel
{
    private readonly IMediator _mediator;

    public CreateModel(IMediator mediator) => _mediator = mediator;

    [BindProperty]
    public CreateProductCommand Command { get; set; } = new("", "", 0m, "USD", null, 0);

    public void OnGet()
    {
        if (string.IsNullOrWhiteSpace(Command.PriceCurrency))
            Command = Command with { PriceCurrency = "USD" };
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var id = await _mediator.Send(Command, ct);

            TempData["SuccessMessage"] = "Product created successfully.";
            return RedirectToPage("Index");
        }
        catch (ConflictException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Unexpected error: {ex.Message}");
            return Page();
        }
    }
}
