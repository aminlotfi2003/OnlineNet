using FluentValidation;
using OnlineNet.Application.Orders.Dtos;

namespace OnlineNet.Application.Orders.Commands.PlaceOrder;

public sealed class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();

        RuleFor(x => x.Items)
            .NotNull()
            .NotEmpty();

        RuleForEach(x => x.Items)
            .SetValidator(new PlaceOrderItemValidator());
    }

    private sealed class PlaceOrderItemValidator : AbstractValidator<PlaceOrderItemDto>
    {
        public PlaceOrderItemValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();

            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}
