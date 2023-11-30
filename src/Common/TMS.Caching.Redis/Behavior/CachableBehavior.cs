using MediatR;

using TMS.Common.Caching;
using TMS.Common.Interfaces;

namespace TMS.Caching.Redis.Behavior;

public sealed class CachableBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICoreCacheClient _coreCacheClient;

    public CachableBehavior(ICoreCacheClient coreCacheClient)
        => _coreCacheClient = coreCacheClient;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is ICachable cachable)
        {
            var key = cachable.GetCacheKey();

            if (request is IQuery<TResponse>)
            {
                return await _coreCacheClient.GetOrAddAsync<TResponse>(
                    key, TimeSpan.FromMinutes(3), () => next());
            }

            if (request is ICommand<TResponse>)
            {
                var result = await next();

                await _coreCacheClient.DeleteAsync(key);

                return result;
            }
        }

        return await next();
    }
}