namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ObservationQueryController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IObservationQueryService _service;

    // todo: perhaps change to observation read?

    public ObservationQueryController(ILogger<ObservationQueryController> logger
                        , IObservationQueryService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetObservationAsync(int id)
    {
        if (id == 0)
        {
            _logger.LogError(LoggingEvents.InvalidOrMissingArgument, $"{nameof(id)} argument is 0");
            return StatusCode(400);
        }

        try
        {
            var observation = await _service.GetObservationAsync(id);

            if (observation is null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, $"observation with id '{id}' was not found.");
                return StatusCode(500);
            }

            return Ok(observation);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetItemNotFound, ex, $"an error occurred getting observation with id '{id}'.");
            return StatusCode(500);
        }
    }

    [HttpGet, Route("species")]
    public async Task<IActionResult> GetObservationsByBirdSpeciesAsync(int birdId, int pageIndex, int pageSize)
    {
        if (birdId == 0)
        {
            _logger.LogError(LoggingEvents.InvalidOrMissingArgument, $"{nameof(birdId)} argument is 0");
            return BadRequest("birdId is zero");
        }

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
        if (string.IsNullOrEmpty(username))
        {
            _logger.LogError(LoggingEvents.InvalidOrMissingArgument, $"{nameof(username)} argument is null or empty");
            return BadRequest("username is null or empty");
        }

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