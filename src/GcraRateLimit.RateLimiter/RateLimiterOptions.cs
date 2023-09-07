using System.ComponentModel.DataAnnotations;

namespace GcraRateLimit.RateLimiter;

public class RateLimiterOptions
{
    /// <summary>
    /// Интервал окна запросов
    /// </summary>
    public TimeSpan Interval { get; set; }
    
    /// <summary>
    /// Максимальное количество запросов в указанное окно (<see cref="Interval"/>)
    /// </summary>
    [Range(0, int.MaxValue)]
    public int MaxRequests { get; set; }
}