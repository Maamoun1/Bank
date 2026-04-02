using BusinessLayer.Service.IService;
using StackExchange.Redis;

namespace InfrastructureLayer.Locking
{
    public class RedisDistributedLock : IDistributedLock
    {

        private readonly IConnectionMultiplexer _redis;

        private const string ReleaseLockScript = @"
            if redis.call('GET', KEYS[1]) == ARGV[1] then
                return redis.call('DEL', KEYS[1])
            else
                return 0
            end";

        public RedisDistributedLock(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
 
        public async Task<string?> AcquireAsync(string key, TimeSpan expiry)
        {
            var db = _redis.GetDatabase();
            var token = Guid.NewGuid().ToString("N"); // compact, no hyphens

            // StringSetAsync with When.NotExists maps directly to SET NX.
            // Returns true  → we own the lock, token is valid.
            // Returns false → another caller holds it, we must not proceed.
            bool acquired = await db.StringSetAsync(key, token, expiry, When.NotExists);

            return acquired ? token : null;
        }

        public async Task ReleaseAsync(string key, string token)
        {
            var db = _redis.GetDatabase();

            await db.ScriptEvaluateAsync(
                ReleaseLockScript,
                keys: new RedisKey[] { key },
                values: new RedisValue[] { token });
        }
    }
}