using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace GcraRateLimit.RateLimiter;

public class RedisGcarRateLimiter: IRateLimiter
{
    private readonly IConnectionMultiplexer _multiplexer;
    private readonly IOptionsMonitor<RateLimiterOptions> _options;

    public RedisGcarRateLimiter(IConnectionMultiplexer multiplexer, IOptionsMonitor<RateLimiterOptions> options)
    {
        _multiplexer = multiplexer;
        _options = options;
    }
    
    public Task<bool> TryGetAccess(string key, CancellationToken token = default)
    {
        return TryGetAccessLocal(key, token);
    }

    /// <summary>
    /// Вся бизнес-логика располагается внутри кода
    /// </summary>
    private async Task<bool> TryGetAccessLocal(string key, CancellationToken token = default)
    {
        var database = _multiplexer.GetDatabase();
        var options = _options.CurrentValue;
        var window = options.Interval / options.MaxRequests;
        return await database.StringSetAsync(key, "access", window, When.NotExists);
    }
    
    /// <summary>
    /// Бизнес-логика расположена в скрипте
    /// </summary>
    private async Task<bool> TryGetAccessScript(string key, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}