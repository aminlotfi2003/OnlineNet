using OnlineNet.Domain.Abstractions;
using OnlineNet.Domain.Products.ValueObjects;

namespace OnlineNet.Domain.Baskets.ValueObjects;

public sealed class BasketItem : ValueObject
{
    public Guid ProductId { get; private init; }
    public string ProductName { get; private init; } = default!;
    public int Quantity { get; private init; }
    public Money UnitPrice { get; private init; } = default!;

    private BasketItem() { }

    public BasketItem(Guid productId, string productName, Money unitPrice, int quantity)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("Product id is required.", nameof(productId));
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required.", nameof(productName));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));
        UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));

        ProductId = productId;
        ProductName = productName.Trim();
        Quantity = quantity;
    }

    public BasketItem IncreaseQuantity(int additionalQuantity)
    {
        if (additionalQuantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(additionalQuantity));

        return WithQuantity(Quantity + additionalQuantity);
    }

    public BasketItem WithQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        return new BasketItem(ProductId, ProductName, UnitPrice, quantity);
    }

    public BasketItem UpdateDetails(string productName, Money unitPrice)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required.", nameof(productName));

        return new BasketItem(ProductId, productName, unitPrice ?? throw new ArgumentNullException(nameof(unitPrice)), Quantity);
    }

    public Money CalculateSubtotal() => UnitPrice.Multiply(Quantity);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProductId;
        yield return ProductName;
        yield return Quantity;
        yield return UnitPrice;
    }
}
