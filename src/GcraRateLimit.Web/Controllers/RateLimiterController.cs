using GcraRateLimit.RateLimiter;
using Microsoft.AspNetCore.Mvc;

namespace GcraRateLimit.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class RateLimiterController : ControllerBase
{
    private readonly IRateLimiter _rateLimiter;
    private readonly ILogger<RateLimiterController> _logger;

    public RateLimiterController(IRateLimiter rateLimiter, ILogger<RateLimiterController> logger)
    {
        _rateLimiter = rateLimiter;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAccessAsync(string key)
    {
        _logger.LogInformation("Получен запрос на получение доступа по ключу {Key}", key);
        if (await _rateLimiter.TryGetAccess(key))
        {
            _logger.LogInformation("Для ключа {Key} доступ получен", key);
            return Ok();
        }

        _logger.LogInformation("Для ключа {Key} доступ не получен", key);
        return BadRequest();
    }
}