using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Library.Application.Data
{
    public class DistributedCacheInvalidationService : ICacheInvalidationService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDistributedCache _cache; 

        public DistributedCacheInvalidationService(IConnectionMultiplexer redis, IDistributedCache cache)
        {
            _redis = redis;
            _cache = cache;
        }

        public Task InvalidateAsync(string key, CancellationToken cancellationToken = default)
        {
            return _cache.RemoveAsync(key, cancellationToken);
        }

        public async Task InvalidateByPatternAsync(string pattern, CancellationToken cancellationToken = default)
        {
            var database = _redis.GetDatabase();

            var endpoints = _redis.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = _redis.GetServer(endpoint);
                var keys = server.Keys(pattern: pattern + "*");

                var tasks = keys.Select(key => database.KeyDeleteAsync(key)).ToArray();
                await Task.WhenAll(tasks);
            }
        }
    }
}
