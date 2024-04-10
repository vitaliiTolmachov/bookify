using Bookify.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Bookify.Application.Behaviours;

public class LoggingCommandBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : Result
{
    private readonly ILogger<LoggingCommandBehaviour<TRequest, TResponse>> _logger;

    public LoggingCommandBehaviour(ILogger<LoggingCommandBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;

        try
        {
            _logger.LogInformation("Request {RequestName}", requestName);
            
            var result = await next();

            if (result.IsSuccess)
            {
                _logger.LogInformation("Request {RequestName} processed successfully", requestName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    _logger.LogError("Request {RequestName} processed with errors", requestName);
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Request {RequestName} processing failed", requestName);
            throw;
        }
    }
}