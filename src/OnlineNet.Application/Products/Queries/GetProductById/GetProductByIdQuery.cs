using OnlineNet.Application.Common.CQRS;
using OnlineNet.Application.Products.Dtos;

namespace OnlineNet.Application.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IQuery<ProductDto>;
