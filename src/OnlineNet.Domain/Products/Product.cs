using OnlineNet.Domain.Abstractions;
using OnlineNet.Domain.Products.ValueObjects;

namespace OnlineNet.Domain.Products;

public sealed class Product : EntityBase
{
    // Required
    public string Name { get; set; } = default!;
    public Sku Sku { get; set; } = default!;
    public Money Price { get; set; } = default!;

    // Optional / operational
    public string? Description { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;

    private Product() { } // EF Core

    public Product(string name, Sku sku, Money price, string? description = null, int stockQuantity = 0)
    {
        SetName(name);
        SetSku(sku);
        SetPrice(price);
        Description = description;
        SetStock(stockQuantity);
    }

    // Behaviors
    public Product Rename(string name)
    {
        SetName(name);
        Touch();
        return this;
    }

    public Product UpdateDescription(string? description)
    {
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Touch();
        return this;
    }

    public Product ChangeSku(Sku sku)
    {
        SetSku(sku);
        Touch();
        return this;
    }

    public Product ChangePrice(Money newPrice)
    {
        SetPrice(newPrice);
        Touch();
        return this;
    }

    public Product IncreaseStock(int count)
    {
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
        StockQuantity += count;
        Touch();
        return this;
    }

    public Product DecreaseStock(int count)
    {
        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count));
        if (count > StockQuantity)
            throw new InvalidOperationException("Insufficient stock.");
        StockQuantity -= count;
        Touch();
        return this;
    }

    public Product Activate()
    {
        IsActive = true;
        Touch();
        return this;
    }

    public Product Deactivate()
    {
        IsActive = false;
        Touch();
        return this;
    }

    // Guards / setters
    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        Name = name.Trim();
    }

    private void SetSku(Sku sku)
    {
        Sku = sku ?? throw new ArgumentNullException(nameof(sku));
    }

    private void SetPrice(Money price)
    {
        Price = price ?? throw new ArgumentNullException(nameof(price));
        if (price.Amount < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(price));
    }

    private void SetStock(int stock)
    {
        if (stock < 0) throw new ArgumentOutOfRangeException(nameof(stock));
        StockQuantity = stock;
    }
}
