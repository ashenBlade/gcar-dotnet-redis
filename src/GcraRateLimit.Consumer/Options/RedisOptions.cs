using System.ComponentModel.DataAnnotations;

namespace GcraRateLimit.Consumer.Options;

public class RedisOptions
{
    [Required]
    [ConfigurationKeyName("HOST")]
    public string Host { get; set; }
}