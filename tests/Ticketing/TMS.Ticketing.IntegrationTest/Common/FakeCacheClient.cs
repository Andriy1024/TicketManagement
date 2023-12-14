using TMS.Common.Caching;

namespace TMS.Ticketing.IntegrationTest.Common;

internal class FakeCacheClient : ICacheClient
{
    public Task<bool> AddAsync<TIn>(string key, TIn item, TimeSpan expireIn)
        => Task.FromResult(true);

    public Task<bool> DeleteAsync(string key) 
        => Task.FromResult(true);

    public Task<TOut?> GetAsync<TOut>(string key) 
        => Task.FromResult(default(TOut));

    public Task<TOut> GetOrAddAsync<TOut>(string key, TimeSpan expireIn, Func<Task<TOut>> factory)
        => factory();

    public Task<TOut> GetOrAddAsync<TOut>(string key, TimeSpan expireIn, Func<TOut> factory)
        => Task.FromResult(factory());
}
