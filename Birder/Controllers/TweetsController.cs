using Microsoft.Extensions.Caching.Memory;

namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class TweetsController : ControllerBase
{
    private readonly ISystemClockService _systemClock;
    private readonly ITweetDataService _service;
    private readonly IMemoryCache _cache;
    private readonly ILogger _logger;

    public TweetsController(IMemoryCache memoryCache,
                            ILogger<TweetsController> logger,
                            ISystemClockService systemClock,
                            ITweetDataService service)
    {
        _cache = memoryCache;
        _logger = logger;
        _systemClock = systemClock;
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetTweetDayAsync()
    {
        try
        {
            if (_cache.TryGetValue(nameof(TweetDayDto), out TweetDayDto tweetDayCache))
            {
                return Ok(tweetDayCache);
            }

            var model = await _service.GetTweetOfTheDayAsync(_systemClock.GetToday);

            if (model is null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "An error occurred getting tweet with date: {Date}", _systemClock.GetToday);
                return StatusCode(500, $"tweets service returned null");
            }

            _cache.Set(nameof(TweetDayDto), model, _systemClock.GetEndOfToday);

            return Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetItemNotFound, ex, "An error occurred getting tweet with date: {Date}", _systemClock.GetToday);
            return StatusCode(500, "an unexpected error occurred");
        }
    }

    [HttpGet, Route("Archive")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTweetArchiveAsync(int pageIndex, int pageSize)
    {
        try
        {
            var model = await _service.GetTweetArchiveAsync(pageIndex, pageSize, _systemClock.GetToday);

            if (model is null)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, "An error occurred getting the tweets archive");
                return StatusCode(500, $"tweets service returned null");
            }

            return Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the tweets archive");
            return StatusCode(500, "an unexpected error occurred");
        }
    }
}