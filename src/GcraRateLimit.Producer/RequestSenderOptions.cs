using System.ComponentModel.DataAnnotations;

namespace GcraRateLimit.Producer;

public class RequestSenderOptions
{
    [Required]
    [ConfigurationKeyName("REQUEST_URL")]
    public Uri RequestUrl { get; set; }

    [Required]
    [ConfigurationKeyName("SLEEP_TIME")]
    public TimeSpan SleepTime { get; set; }
}