namespace OnlineNet.Application.Orders.Dtos;

public sealed record OrderSummaryDto(
    Guid Id,
    Guid CustomerId,
    string Status,
    decimal TotalAmount,
    string Currency,
    DateTimeOffset CreatedOn,
    IReadOnlyCollection<OrderLineDto> Lines
);
