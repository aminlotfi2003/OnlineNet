using OnlineNet.Domain.Abstractions;
using OnlineNet.Domain.Products.ValueObjects;

namespace OnlineNet.Domain.Orders.ValueObjects;

public sealed class OrderLine : ValueObject
{
    public Guid ProductId { get; private init; }
    public string ProductName { get; private init; } = default!;
    public int Quantity { get; private init; }
    public Money UnitPrice { get; private init; } = default!;

    private OrderLine() { }

    public OrderLine(Guid productId, string productName, int quantity, Money unitPrice)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("Product id is required.", nameof(productId));
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required.", nameof(productName));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        ProductId = productId;
        ProductName = productName.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));
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
