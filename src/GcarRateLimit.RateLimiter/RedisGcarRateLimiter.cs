using StackExchange.Redis;

namespace GcarRateLimit.RateLimiter;

public class RedisGcarRateLimiter: IRateLimiter
{
    private readonly IConnectionMultiplexer _multiplexer;

    public RedisGcarRateLimiter(IConnectionMultiplexer multiplexer)
    {
        _multiplexer = multiplexer;
    }
    
    public async Task<bool> TryGetAccess(string key, CancellationToken token = default)
    {
        var database = _multiplexer.GetDatabase();
        throw new NotImplementedException();
    }
}