namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ObservationAnalysisController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IObservationsAnalysisService _observationsAnalysisService;

    public ObservationAnalysisController(ILogger<ObservationAnalysisController> logger
                                        , IObservationsAnalysisService observationsAnalysisService)
    {
        _observationsAnalysisService = observationsAnalysisService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetObservationCountAsync()
    {
        //if (string.IsNullOrEmpty(requestedUsername))
        //{
        //    _logger.LogError(LoggingEvents.InvalidOrMissingArgument, $"{nameof(requestedUsername)} argument is null or empty");
        //    return BadRequest("requestedUsername is missing");
        //}

        try
        {
            var requestingUsername = User.Identity.Name;

            if (string.IsNullOrEmpty(requestingUsername))
            {
                string errorMessage = $"requesting username is null or empty";
                _logger.LogError(LoggingEvents.GetListNotFound, errorMessage);
                return Unauthorized(errorMessage);
            }

            var viewModel = await _observationsAnalysisService.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == requestingUsername);

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the Observations Analysis");
            return StatusCode(500, "an unexpected error occurred");
        }
    }

    [HttpGet, Route("User")]
    public async Task<IActionResult> GetObservationAnalysisAsync(string requestedUsername)
    {
        if (string.IsNullOrEmpty(requestedUsername))
        {
            _logger.LogError(LoggingEvents.InvalidOrMissingArgument, $"{nameof(requestedUsername)} argument is null or empty");
            return BadRequest("requestedUsername is missing");
        }

        try
        {
            var viewModel = await _observationsAnalysisService.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == requestedUsername);

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the Observations Analysis");
            return StatusCode(500, "an unexpected error occurred");
        }
    }
}