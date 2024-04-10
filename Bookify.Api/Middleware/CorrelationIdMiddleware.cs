using Serilog.Context;

namespace Bookify.Api.Middleware;

public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeader = "X-Correlation-Id";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public Task InvokeAsync(HttpContext context)
    {
        using (LogContext.PushProperty("CorrelationId", ReuseOrGenerateCorrelationId(context)))
        {
            return _next.Invoke(context);
        }
    }

    private string ReuseOrGenerateCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(CorrelationIdHeader, out var requestCorrelationId);
        return requestCorrelationId.FirstOrDefault() ?? context.TraceIdentifier;
    }
}