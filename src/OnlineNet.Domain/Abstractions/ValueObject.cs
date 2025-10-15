namespace OnlineNet.Domain.Abstractions;

/// <summary>
/// Base class for all Value Objects in the domain model.
/// Implements equality comparison based on atomic values.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    /// <summary>
    /// Derived classes must yield all atomic values that define equality.
    /// </summary>
    /// <returns>Sequence of equality components.</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (GetType() != obj.GetType())
            return false;

        var other = (ValueObject)obj;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public bool Equals(ValueObject? other) => Equals((object?)other);

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(0, (hash, component) =>
            {
                unchecked
                {
                    return (hash * 31) + (component?.GetHashCode() ?? 0);
                }
            });
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null)
            return true;
        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);

    /// <summary>
    /// Creates a shallow copy of the ValueObject.
    /// </summary>
    public ValueObject GetCopy() => (ValueObject)MemberwiseClone();
}
