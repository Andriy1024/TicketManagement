using StackExchange.Redis.Extensions.Core.Abstractions;

using TMS.Common.Caching;

namespace TMS.Caching.Redis;

public class CoreCacheClient<TDataBase> : ICoreCacheClient
    where TDataBase : DbNumber, new()
{
    private readonly IRedisDatabase _database;

    public CoreCacheClient(IRedisClient redisClient)
        => _database = redisClient.GetDb(new TDataBase().Value);

    public Task<TOut?> GetAsync<TOut>(string key)
        => _database.GetAsync<TOut?>(key);

    public Task<bool> AddAsync<TIn>(string key, TIn item, TimeSpan expiresIn)
        => _database.AddAsync(key, item, expiresIn);

    public Task<bool> DeleteAsync(string key)
        => _database.RemoveAsync(key);

    public async Task<TOut> GetOrAddAsync<TOut>(string key, TimeSpan expiresIn, Func<Task<TOut>> factory)
    {
        var item = await _database.GetAsync<TOut>(key);

        if (item == null)
        {
            item = await factory();

            await _database.AddAsync(key, item, expiresIn);
        }

        return item;
    }

    public async Task<TOut> GetOrAddAsync<TOut>(string key, TimeSpan expiresIn, Func<TOut> factory)
    {
        var item = await _database.GetAsync<TOut>(key);

        if (item == null)
        {
            item = factory();

            await _database.AddAsync(key, item, expiresIn);
        }

        return item;
    }
}