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
            var viewModel = await _service.GetPagedObservationsAsync(cs => cs.BirdId == birdId, pageIndex, pageSize);

            if (viewModel is null)
            {
                string message = $"Observations with birdId '{birdId}' was not found.";
                _logger.LogWarning(LoggingEvents.GetListNotFound, message);
                return StatusCode(500, message);
            }

            return Ok(viewModel);
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
            var viewModel = await _service.GetPagedObservationsAsync(o => o.ApplicationUser.UserName == username, pageIndex, pageSize);

            if (viewModel is null)
            {
                string message = $"Observations with username '{username}' was not found.";
                _logger.LogWarning(LoggingEvents.GetListNotFound, message);
                return StatusCode(500, message);
            }

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, ex.Message);
            return StatusCode(500, "an unexpected error occurred");
        }
    }
}