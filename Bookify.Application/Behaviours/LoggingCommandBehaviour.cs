using Bookify.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Behaviours;

public class LoggingCommandBehaviour<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse>
    where TCommand : IBaseCommand
{
    private readonly ILogger<TCommand> _logger;

    public LoggingCommandBehaviour(ILogger<TCommand> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var commandType = request.GetType().Name;

        try
        {
            _logger.LogInformation("Executing command {Command}", commandType);
            
            var result = await next();
            
            _logger.LogInformation("Command {Command} processed successfully", commandType);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Command {Command} processing failed", commandType);
            throw;
        }
    }
}