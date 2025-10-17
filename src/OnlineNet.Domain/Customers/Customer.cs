using OnlineNet.Domain.Abstractions;
using OnlineNet.Domain.Customers.ValueObjects;

namespace OnlineNet.Domain.Customers;

public sealed class Customer : EntityBase
{
    public CustomerName Name { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public Guid? ActiveBasketId { get; private set; }

    private Customer() { }

    public Customer(CustomerName name, Email email)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public Customer Rename(CustomerName name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Touch();
        return this;
    }

    public Customer ChangeEmail(Email email)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Touch();
        return this;
    }

    public void AssignBasket(Guid basketId)
    {
        if (basketId == Guid.Empty)
            throw new ArgumentException("Basket id is required.", nameof(basketId));

        ActiveBasketId = basketId;
        Touch();
    }

    public void ClearBasket()
    {
        ActiveBasketId = null;
        Touch();
    }
}
