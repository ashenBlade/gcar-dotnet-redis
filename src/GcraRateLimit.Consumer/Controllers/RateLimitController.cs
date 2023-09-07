using System.Net;
using GcraRateLimit.RateLimiter;
using Microsoft.AspNetCore.Mvc;

namespace GcraRateLimit.Consumer.Controllers;

[ApiController]
[Route("/rate-limit")]
public class RateLimitController : ControllerBase
{
    private readonly IRateLimiter _rateLimiter;
    private readonly ILogger<RateLimitController> _logger;

    public RateLimitController(IRateLimiter rateLimiter, ILogger<RateLimitController> logger)
    {
        _rateLimiter = rateLimiter;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAccessAsync(string key)
    {
        if (await _rateLimiter.TryGetAccess(key))
        {
            Counters.SuccessRequests.Add(1);
            return Ok();
        }

        return StatusCode((int)HttpStatusCode.TooManyRequests);
    }
}