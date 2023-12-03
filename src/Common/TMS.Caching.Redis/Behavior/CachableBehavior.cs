using MediatR;

using TMS.Common.Caching;

namespace TMS.Caching.Redis.Behavior;

internal sealed class CachableBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheClient _cache;

    public CachableBehavior(ICacheClient coreCacheClient)
        => _cache = coreCacheClient;

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        => request is ICachable cachable
            ? _cache.GetOrAddAsync(cachable.GetCacheKey(), TimeSpan.FromMinutes(3), () => next())
            : next();
}