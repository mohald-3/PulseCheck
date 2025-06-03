using Application.Interfaces.Services;
using MediatR;
using System.Text.Json;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IRequestLogger _logger;

    public LoggingBehavior(IRequestLogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest).Name;
        var requestJson = JsonSerializer.Serialize(request);

        var response = await next();

        var responseJson = JsonSerializer.Serialize(response);

        await _logger.LogAsync(requestType, requestJson, responseJson);

        return response;
    }
}
