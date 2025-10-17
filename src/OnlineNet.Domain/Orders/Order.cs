using OnlineNet.Domain.Abstractions;
using OnlineNet.Domain.Orders.ValueObjects;
using OnlineNet.Domain.Products.ValueObjects;

namespace OnlineNet.Domain.Orders;

public sealed class Order : EntityBase
{
    private readonly List<OrderLine> _lines = new();

    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public Money Total { get; private set; } = default!;

    public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();

    private Order() { }

    private Order(Guid customerId, IEnumerable<OrderLine> lines)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("Customer id is required.", nameof(customerId));

        CustomerId = customerId;
        Status = OrderStatus.Placed;
        SetLines(lines);
    }

    public static Order Create(Guid customerId, IEnumerable<OrderLine> lines)
        => new(customerId, lines);

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Placed)
            throw new InvalidOperationException("Only placed orders can be marked as paid.");

        Status = OrderStatus.Paid;
        Touch();
    }

    public void MarkAsShipped()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException("Only paid orders can be shipped.");

        Status = OrderStatus.Shipped;
        Touch();
    }

    public void Complete()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException("Only shipped orders can be completed.");

        Status = OrderStatus.Completed;
        Touch();
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Completed orders cannot be cancelled.");

        Status = OrderStatus.Cancelled;
        Touch();
    }

    private void SetLines(IEnumerable<OrderLine> lines)
    {
        ArgumentNullException.ThrowIfNull(lines);

        var lineList = lines.ToList();
        if (lineList.Count == 0)
            throw new ArgumentException("An order must contain at least one line.", nameof(lines));

        var currency = lineList[0].UnitPrice.Currency;

        _lines.Clear();
        foreach (var line in lineList)
        {
            if (line.UnitPrice.Currency != currency)
                throw new InvalidOperationException("All order lines must share the same currency.");

            _lines.Add(line);
        }

        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        var currency = _lines[0].UnitPrice.Currency;
        var total = Money.From(0, currency);
        foreach (var line in _lines)
        {
            total = total.Add(line.CalculateSubtotal());
        }

        Total = total;
    }
}
