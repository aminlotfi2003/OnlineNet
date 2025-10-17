using OnlineNet.Domain.Abstractions;
using OnlineNet.Domain.Baskets.ValueObjects;
using OnlineNet.Domain.Products.ValueObjects;

namespace OnlineNet.Domain.Baskets;

public sealed class Basket : EntityBase
{
    private readonly List<BasketItem> _items = new();

    public Guid CustomerId { get; private set; }
    public bool IsLocked { get; private set; }

    public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

    private Basket() { }

    public Basket(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("Customer id is required.", nameof(customerId));

        CustomerId = customerId;
    }

    public void AddOrUpdateItem(Guid productId, string productName, Money unitPrice, int quantity)
    {
        EnsureUnlocked();

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        unitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));

        var existingIndex = _items.FindIndex(i => i.ProductId == productId);
        if (existingIndex >= 0)
        {
            var updated = _items[existingIndex]
                .UpdateDetails(productName, unitPrice)
                .IncreaseQuantity(quantity);
            _items[existingIndex] = updated;
        }
        else
        {
            _items.Add(new BasketItem(productId, productName, unitPrice, quantity));
        }

        Touch();
    }

    public void RemoveItem(Guid productId)
    {
        EnsureUnlocked();

        var index = _items.FindIndex(i => i.ProductId == productId);
        if (index >= 0)
        {
            _items.RemoveAt(index);
            Touch();
        }
    }

    public void Clear()
    {
        EnsureUnlocked();

        if (_items.Count == 0) return;

        _items.Clear();
        Touch();
    }

    public void Lock()
    {
        IsLocked = true;
        Touch();
    }

    public void Unlock()
    {
        IsLocked = false;
        Touch();
    }

    private void EnsureUnlocked()
    {
        if (IsLocked)
            throw new InvalidOperationException("The basket is locked and cannot be modified.");
    }
}
