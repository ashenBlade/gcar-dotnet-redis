using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace GcraRateLimit.RateLimiter;

public class RedisGcraRateLimiter: IRateLimiter
{
    private readonly IConnectionMultiplexer _multiplexer;
    private readonly IDatabase _database;
    private readonly IOptionsMonitor<RateLimiterOptions> _options;
    private readonly TimeSpan _window;
    
    public RedisGcraRateLimiter(IConnectionMultiplexer multiplexer, IOptionsMonitor<RateLimiterOptions> options)
    {
        _multiplexer = multiplexer;
        _options = options;
        _window = options.CurrentValue.Interval / options.CurrentValue.MaxRequests - TimeSpan.FromMilliseconds(10);
        _database = _multiplexer.GetDatabase();
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
        return await _database.StringSetAsync(key, KeyValue, _window, When.NotExists);
    }

    private static readonly RedisValue KeyValue = new("access");
    
    /// <summary>
    /// Бизнес-логика расположена в скрипте
    /// </summary>
    private async Task<bool> TryGetAccessScript(string key, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}