using MediatR;
using OnlineNet.Application.Common.CQRS;

namespace OnlineNet.Application.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id) : ICommand<Unit>;
