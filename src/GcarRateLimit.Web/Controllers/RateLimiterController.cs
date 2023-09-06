using GcarRateLimit.RateLimiter;
using Microsoft.AspNetCore.Mvc;

namespace GcarRateLimit.Web.Controllers;

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
        if (await _rateLimiter.TryGetAccess(key))
        {
            return Ok();
        }

        return BadRequest();
    }
}