using System.ComponentModel.DataAnnotations;

namespace GcraRateLimit.Producer.Options;

public class RedisOptions
{
    [Required]
    [ConfigurationKeyName("HOST")]
    public string Host { get; set; }
}