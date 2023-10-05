namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class BirdsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IBirdDataService _service;

    public BirdsController(ILogger<BirdsController> logger
                         , IBirdDataService service)
    {
        _logger = logger;
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
                _logger.LogWarning(LoggingEvents.GetListNotFound, "birds list was null");
                return StatusCode(500, $"bird service returned null");
            }

            return Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, "an error occurred getting the birds list");
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
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "GetBird({id}) returned null", id);
                return StatusCode(500, $"bird service returned null");
            }

            return Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetItemNotFound, ex, "an error occurred getting bird with {id}", id);
            return StatusCode(500, "an unexpected error occurred");
        }
    }
}