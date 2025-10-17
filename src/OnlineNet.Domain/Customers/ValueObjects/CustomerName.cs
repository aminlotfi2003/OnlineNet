using OnlineNet.Domain.Abstractions;

namespace OnlineNet.Domain.Customers.ValueObjects;

public sealed class CustomerName : ValueObject
{
    public string FirstName { get; private init; } = default!;
    public string LastName { get; private init; } = default!;

    private CustomerName() { }

    public CustomerName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("First name is required.", nameof(lastName));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    public string FullName => $"{FirstName} {LastName}";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }

    public override string ToString() => FullName;
}
