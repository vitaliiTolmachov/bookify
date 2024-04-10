using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Behaviours;

public class QueryCachingBehaviour<TQuery, TResponse> : IPipelineBehavior<TQuery, TResponse>
    where TQuery : ICachedQuery
    where TResponse : Result
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<QueryCachingBehaviour<TQuery, TResponse>> _logger;

    public QueryCachingBehaviour(
        ICacheService cacheService,
        ILogger<QueryCachingBehaviour<TQuery, TResponse>> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }
    
    public async  Task<TResponse> Handle(
        TQuery request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var queryName = request.GetType().Name;
        
        var cachedResult = await _cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            _logger.LogInformation("Cache hit for query {QueryName} with key {CacheKey}", queryName, request.CacheKey);
            return cachedResult;
        }
        
        _logger.LogInformation("Cache is missing for {QueryName} with key {CacheKey}", queryName,  request.CacheKey);

        var result = await next.Invoke();

        if (result.IsSuccess)
        {
            await _cacheService.SetAsync(request.CacheKey, result, request.Expiration, cancellationToken);
        }

        return result;
    }
}