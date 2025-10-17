namespace OnlineNet.Application.Customers.Dtos;

public sealed record CustomerDetailDto(
    Guid Id,
    string FullName,
    string Email,
    Guid? ActiveBasketId,
    DateTimeOffset CreatedOn,
    DateTimeOffset? ModifiedOn
);
