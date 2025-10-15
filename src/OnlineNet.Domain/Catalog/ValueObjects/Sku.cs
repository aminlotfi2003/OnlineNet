using OnlineNet.Domain.Abstractions;

namespace OnlineNet.Domain.Catalog.ValueObjects;

public sealed class Sku : ValueObject
{
    public string Value { get; private init; } = default!;

    private Sku() { } // EF Core

    public Sku(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SKU value is required.", nameof(value));

        if (value.Length < 3 || value.Length > 20)
            throw new ArgumentException("SKU must be between 3 and 20 characters.", nameof(value));

        Value = value.ToUpperInvariant();
    }

    public static Sku From(string value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
