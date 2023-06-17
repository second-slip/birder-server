namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ObservationQueryController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IObservationQueryService _service;

    public ObservationQueryController(ILogger<ObservationQueryController> logger
                        , IObservationQueryService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet, Route("species")]
    public async Task<IActionResult> GetObservationsByBirdSpeciesAsync(int birdId, int pageIndex, int pageSize)
    {
        try
        {
            var model = await _service.GetPagedObservationsAsync(cs => cs.BirdId == birdId, pageIndex, pageSize);

            if (model is null)
            {
                _logger.LogWarning(LoggingEvents.GetListNotFound, $"null object returned by ${nameof(IObservationQueryService)}");
                return StatusCode(500, "an unexpected error occurred");
            }

            return Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, ex.Message);
            return StatusCode(500, "an unexpected error occurred");
        }
    }

    [HttpGet, Route("user")]
    public async Task<IActionResult> GetObservationsByUserAsync(string username, int pageIndex, int pageSize)
    {
        try
        {
            var model = await _service.GetPagedObservationsAsync(o => o.ApplicationUser.UserName == username, pageIndex, pageSize);

            if (model is null)
            {
                _logger.LogWarning(LoggingEvents.GetListNotFound, $"null object returned by ${nameof(IObservationQueryService)}");
                return StatusCode(500, "an unexpected error occurred");
            }

            return Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, ex.Message);
            return StatusCode(500, "an unexpected error occurred");
        }
    }
}