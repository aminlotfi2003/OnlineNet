using OnlineNet.Domain.Abstractions;
using System.Net.Mail;

namespace OnlineNet.Domain.Customers.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; private init; } = default!;

    private Email() { }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email is required.", nameof(value));

        value = value.Trim();
        if (!IsValid(value))
            throw new ArgumentException("Email format is invalid.", nameof(value));

        Value = value.ToLowerInvariant();
    }

    private static bool IsValid(string value)
    {
        try
        {
            _ = new MailAddress(value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
