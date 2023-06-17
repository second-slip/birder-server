﻿namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ObservationAnalysisController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IObservationsAnalysisService _service;

    public ObservationAnalysisController(ILogger<ObservationAnalysisController> logger
                                       , IObservationsAnalysisService service)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetObservationCountAsync()
    {
        var requestingUsername = User.Identity.Name;

        if (string.IsNullOrEmpty(requestingUsername))
        {
            _logger.LogError(LoggingEvents.GetListNotFound, "requesting username is null or empty");
            return StatusCode(500, "an unexpected error occurred");
        }
        
        try
        {
            var viewModel = await _service.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == requestingUsername);
            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, ex.Message);
            return StatusCode(500, "an unexpected error occurred");
        }
    }

    [HttpGet, Route("user")]
    public async Task<IActionResult> GetObservationAnalysisAsync(string requestedUsername)
    {
        if (string.IsNullOrEmpty(requestedUsername))
        {
            _logger.LogError(LoggingEvents.InvalidOrMissingArgument, $"{nameof(requestedUsername)} argument is null or empty");
            return StatusCode(500, "requesting username is null or empty");
        }

        try
        {
            var viewModel = await _service.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == requestedUsername);
            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, ex.Message);
            return StatusCode(500, "an unexpected error occurred");
        }
    }
}