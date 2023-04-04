using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class BirdsController : ControllerBase
{
    private readonly IMemoryCache _cache;
    private readonly ILogger _logger;
    private readonly IBirdDataService _service;

    public BirdsController(IMemoryCache memoryCache
                         , ILogger<BirdsController> logger
                         , IBirdDataService service)
    {
        _logger = logger;
        _cache = memoryCache;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetBirdsAsync(int pageIndex, int pageSize, BirderStatus speciesFilter)
    {
        try
        {
            var model = await _service.GetBirdsAsync(pageIndex, pageSize, speciesFilter);

            if (model is null)
            {
                _logger.LogWarning(LoggingEvents.GetListNotFound, "Birds list is null");
                return StatusCode(500, $"bird service returned null");
            }

            return Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the birds list");
            return StatusCode(500, "an unexpected error occurred");
        }
    }

    [HttpGet, Route("BirdsList")]
    public async Task<IActionResult> GetBirdsDdlAsync()
    {
        try
        {
            if (_cache.TryGetValue(CacheEntries.BirdsSummaryList, out IEnumerable<BirdSummaryDto> birdsCache))
            {
                return Ok(birdsCache);
            }
            else
            {
                var model = await _service.GetBirdsListAsync();

                if (model is null)
                {
                    _logger.LogWarning(LoggingEvents.GetListNotFound, "Birds list is null");
                    return StatusCode(500, $"bird service returned null");
                }

                _cache.Set(CacheEntries.BirdsSummaryList, model, TimeSpan.FromDays(1));

                return Ok(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the birds list");
            return StatusCode(500, "an unexpected error occurred");
        }
    }

    [HttpGet, Route("Bird")]
    public async Task<IActionResult> GetBirdAsync(int id)
    {
        try
        {
            var model = await _service.GetBirdAsync(id);

            if (model is null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "GetBird({ID}) NOT FOUND", id);
                return StatusCode(500, $"bird service returned null");
            }

            return Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetItemNotFound, ex, "An error occurred getting bird with {ID}", id);
            return StatusCode(500, "an unexpected error occurred");
        }
    }
}