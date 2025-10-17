namespace OnlineNet.Application.Orders.Dtos;

public sealed record OrderListItemDto(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    string Status,
    decimal TotalAmount,
    string Currency,
    DateTimeOffset CreatedOn,
    int LineCount
);
