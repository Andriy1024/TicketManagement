using MediatR;

using TMS.Common.Caching;
using TMS.Common.Interfaces;

namespace TMS.Caching.Redis.Behavior;

public sealed class CachableBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICoreCache _cache;

    public CachableBehavior(ICoreCache coreCacheClient)
        => _cache = coreCacheClient;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is ICachable cachable)
        {
            var key = cachable.GetCacheKey();

            if (request is IQuery<TResponse>)
            {
                return await _cache.GetOrAddAsync(
                    key, TimeSpan.FromMinutes(3), () => next());
            }

            if (request is ICommand<TResponse>)
            {
                var result = await next();

                await _cache.DeleteAsync(key);

                return result;
            }
        }

        return await next();
    }
}