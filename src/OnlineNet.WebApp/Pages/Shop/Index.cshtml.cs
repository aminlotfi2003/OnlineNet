using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineNet.Application.Baskets.Dtos;
using OnlineNet.Application.Baskets.Queries.GetCustomerBasket;
using OnlineNet.Application.Customers.Dtos;
using OnlineNet.Application.Customers.Queries.ListCustomers;
using OnlineNet.Application.Orders.Commands.PlaceOrder;
using OnlineNet.Application.Orders.Dtos;
using OnlineNet.Application.Orders.Queries.GetCustomerOrders;
using OnlineNet.Application.Products.Dtos;
using OnlineNet.Application.Products.Queries.ListProducts;
using System.ComponentModel.DataAnnotations;

namespace OnlineNet.WebApp.Pages.Shop;

public class IndexModel : PageModel
{
    private readonly IMediator _mediator;

    public IndexModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public List<CustomerSummaryDto> Customers { get; private set; } = [];
    public List<ProductDto> Products { get; private set; } = [];
    public BasketDto? Basket { get; private set; }
    public List<OrderSummaryDto> Orders { get; private set; } = [];

    [BindProperty(SupportsGet = true)]
    public Guid? CustomerId { get; set; }

    [BindProperty]
    public PlaceOrderInput Input { get; set; } = new();

    public string? SuccessMessage
    {
        get => TempData[nameof(SuccessMessage)] as string;
        set => TempData[nameof(SuccessMessage)] = value;
    }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        await LoadCommonDataAsync(cancellationToken);

        if (CustomerId is { } customerId && customerId != Guid.Empty)
        {
            Input.CustomerId = customerId;
            await LoadCustomerDetailsAsync(customerId, cancellationToken);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (Input.CustomerId == Guid.Empty)
        {
            ModelState.AddModelError($"{nameof(Input)}.{nameof(Input.CustomerId)}", "Customer is required.");
        }

        if (Input.ProductId == Guid.Empty)
        {
            ModelState.AddModelError($"{nameof(Input)}.{nameof(Input.ProductId)}", "Product is required.");
        }

        await LoadCommonDataAsync(cancellationToken);

        if (!ModelState.IsValid)
        {
            if (Input.CustomerId != Guid.Empty)
            {
                CustomerId = Input.CustomerId;
                await LoadCustomerDetailsAsync(Input.CustomerId, cancellationToken);
            }

            return Page();
        }

        var command = new PlaceOrderCommand(
            Input.CustomerId,
            new List<PlaceOrderItemDto>
            {
                new(Input.ProductId, Input.Quantity)
            });

        await _mediator.Send(command, cancellationToken);

        SuccessMessage = "Order placed successfully.";

        return RedirectToPage(new { customerId = Input.CustomerId });
    }

    private async Task LoadCommonDataAsync(CancellationToken cancellationToken)
    {
        Customers = await _mediator.Send(new ListCustomersQuery(), cancellationToken);
        var products = await _mediator.Send(new ListProductsQuery(), cancellationToken);
        Products = products
            .Where(p => p.IsActive && p.StockQuantity > 0)
            .OrderBy(p => p.Name)
            .ToList();
    }

    private async Task LoadCustomerDetailsAsync(Guid customerId, CancellationToken cancellationToken)
    {
        Basket = await _mediator.Send(new GetCustomerBasketQuery(customerId), cancellationToken);
        Orders = await _mediator.Send(new GetCustomerOrdersQuery(customerId), cancellationToken);
    }

    public sealed class PlaceOrderInput
    {
        [Display(Name = "Customer")]
        public Guid CustomerId { get; set; }

        [Display(Name = "Product")]
        public Guid ProductId { get; set; }

        [Range(1, 1000)]
        public int Quantity { get; set; } = 1;
    }

    public IEnumerable<SelectListItem> CustomerItems =>
    Customers.Select(c => new SelectListItem
    {
        Value = c.Id.ToString(),
        Text = $"{c.FullName} ({c.Email})"
    });

    public IEnumerable<SelectListItem> ProductItems =>
        Products.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = $"{p.Name} ({p.PriceAmount:N2} {p.PriceCurrency}, Stock: {p.StockQuantity})"
        });

}
