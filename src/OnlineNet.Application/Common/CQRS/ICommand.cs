using MediatR;

namespace OnlineNet.Application.Common.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse> { }
public interface ICommand : IRequest { }
