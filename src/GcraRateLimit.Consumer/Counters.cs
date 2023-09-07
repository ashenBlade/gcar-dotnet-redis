using System.Diagnostics.Metrics;

namespace GcraRateLimit.Producer;

public static class Counters
{
    public static readonly Meter AppMeter = new(typeof(Program).Assembly.FullName!);
    public static readonly Counter<long> SuccessRequests = AppMeter.CreateCounter<long>(
        name: "rate-limit-hit-success",
        unit: null,
        description: "Количество успешных попыток получения доступа у Rate Limiter");
}