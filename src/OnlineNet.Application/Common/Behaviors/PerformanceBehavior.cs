using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace OnlineNet.Application.Common.Behaviors;

public sealed class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();

        if (sw.ElapsedMilliseconds > 500) // slow request threshold
            _logger.LogWarning("Long Running Request: {RequestName} ({Elapsed} ms) {@Request}",
                typeof(TRequest).Name, sw.ElapsedMilliseconds, request);

        return response;
    }
}
