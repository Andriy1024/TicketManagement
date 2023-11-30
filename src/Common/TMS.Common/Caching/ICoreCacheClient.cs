namespace TMS.Common.Caching;

public interface ICoreCacheClient
{
    Task<TOut?> GetAsync<TOut>(string key);

    Task<bool> AddAsync<TIn>(string key, TIn item, TimeSpan expireIn);

    Task<bool> DeleteAsync(string key);

    Task<TOut> GetOrAddAsync<TOut>(string key, TimeSpan expireIn, Func<Task<TOut>> factory);

    Task<TOut> GetOrAddAsync<TOut>(string key, TimeSpan expireIn, Func<TOut> factory);
}