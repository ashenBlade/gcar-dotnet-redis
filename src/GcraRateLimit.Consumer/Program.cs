using GcraRateLimit.Consumer;
using GcraRateLimit.Consumer.Options;
using GcraRateLimit.RateLimiter;
using Microsoft.Extensions.Options;
using OpenTelemetry.Metrics;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var options = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
    Console.WriteLine($"Подключаюсь к {options.Host}");
    var multiplexer = ConnectionMultiplexer.Connect(options.Host);
    Console.WriteLine($"Подключился");
    return multiplexer;
});
builder.Services
       .AddOptions<RedisOptions>()
       .Bind(builder.Configuration.GetRequiredSection("REDIS"))
       .ValidateDataAnnotations()
       .ValidateOnStart();


builder.Services
       .AddOpenTelemetry()
       .WithMetrics(m =>
        {
            m.AddPrometheusExporter();
            m.AddMeter(Counters.AppMeter.Name);
        });

builder.Services.AddSingleton<IRateLimiter, RedisGcarRateLimiter>();

builder.Services
       .AddOptions<RateLimiterOptions>()
       .Bind(builder.Configuration.GetRequiredSection("RATE_LIMIT"))
       .ValidateDataAnnotations()
       .ValidateOnStart();

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");

app.MapControllers();

try
{
    app.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}