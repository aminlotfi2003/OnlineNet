using MediatR;

namespace OnlineNet.Application.Common.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse> { }
