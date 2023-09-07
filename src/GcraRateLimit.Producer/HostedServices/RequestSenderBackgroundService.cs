using GcraRateLimit.RateLimiter;
using Microsoft.Extensions.Options;

namespace GcraRateLimit.Producer.HostedServices;

public class RequestSenderBackgroundService: BackgroundService
{
    private readonly IRateLimiter _rateLimiter;
    private readonly IOptionsMonitor<RequestSenderOptions> _options;
    private readonly ILogger<RequestSenderBackgroundService> _logger;

    public RequestSenderBackgroundService(
        IRateLimiter rateLimiter,
        IOptionsMonitor<RequestSenderOptions> options, 
        ILogger<RequestSenderBackgroundService> logger)
    {
        _rateLimiter = rateLimiter;
        _options = options;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var client = new HttpClient();
        _logger.LogInformation("Начинаю отправлять запросы на адрес {Address}", _options.CurrentValue.RequestUrl);
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogDebug("Посылаю очередной запрос");
                    await GainAccess(stoppingToken);
                    if (stoppingToken.IsCancellationRequested)
                    {
                        break;
                    }
                    
                    var response = await client.GetAsync(_options.CurrentValue.RequestUrl, stoppingToken);
                    response.Dispose();
                }
                catch (HttpRequestException e)
                {
                    _logger.LogDebug(e, "Ошибка отправки запроса к узлу");
                    await Task.Delay(_options.CurrentValue.SleepTime, stoppingToken);
                }
            }
        }
        catch (OperationCanceledException)
        { }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Необработанное исключение во время работы");
            throw;
        }
        _logger.LogInformation("Заканчиваю работу");
    }

    private async Task GainAccess(CancellationToken token)
    {
        while (!( await _rateLimiter.TryGetAccess("access", token) || token.IsCancellationRequested ))
        {
            continue;
        }
    }
}