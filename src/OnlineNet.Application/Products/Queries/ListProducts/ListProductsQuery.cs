using OnlineNet.Application.Common.CQRS;
using OnlineNet.Application.Products.Dtos;

namespace OnlineNet.Application.Products.Queries.ListProducts;

public sealed record ListProductsQuery() : IQuery<List<ProductDto>>;
