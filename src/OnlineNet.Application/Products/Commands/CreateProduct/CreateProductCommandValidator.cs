using FluentValidation;

namespace OnlineNet.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Sku).NotEmpty().Length(3, 20);
        RuleFor(x => x.PriceCurrency).NotEmpty().Length(3, 3);
        RuleFor(x => x.PriceAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Description).MaximumLength(2000).When(x => x.Description is not null);
    }
}
