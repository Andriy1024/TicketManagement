using StackExchange.Redis.Extensions.Core.Abstractions;

using TMS.Common.Caching;

namespace TMS.Caching.Redis;

public class CoreCache<TDataBase> : ICoreCache
    where TDataBase : DBNumber, new()
{
    private readonly IRedisDatabase _db;

    public CoreCache(IRedisClient redisClient)
        => _db = redisClient.GetDb(new TDataBase().Value);

    public Task<TOut?> GetAsync<TOut>(string key)
        => _db.GetAsync<TOut?>(key);

    public Task<bool> AddAsync<TIn>(string key, TIn item, TimeSpan expiresIn)
        => _db.AddAsync(key, item, expiresIn);

    public Task<bool> DeleteAsync(string key)
        => _db.RemoveAsync(key);

    public async Task<TOut> GetOrAddAsync<TOut>(string key, TimeSpan expiresIn, Func<Task<TOut>> factory)
    {
        var item = await _db.GetAsync<TOut>(key);

        if (item == null)
        {
            item = await factory();

            await _db.AddAsync(key, item, expiresIn);
        }

        return item;
    }

    public async Task<TOut> GetOrAddAsync<TOut>(string key, TimeSpan expiresIn, Func<TOut> factory)
    {
        var item = await _db.GetAsync<TOut>(key);

        if (item == null)
        {
            item = factory();

            await _db.AddAsync(key, item, expiresIn);
        }

        return item;
    }
}