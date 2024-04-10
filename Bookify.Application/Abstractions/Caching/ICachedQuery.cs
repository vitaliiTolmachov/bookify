using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Abstractions.Caching;

public interface ICachedQuery
{
    public string CacheKey { get; }

    public TimeSpan? Expiration { get; }
}

public interface ICachedQuery<TResult> : IQuery<TResult>, ICachedQuery;