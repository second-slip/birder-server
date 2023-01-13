namespace Birder.Controllers;  // .NET6 / C#10 top-level statement

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ListController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IListService _listService;
    private readonly ISystemClockService _systemClock;

    public ListController(ILogger<ListController> logger
                        , ISystemClockService systemClock
                        , IListService listService)
    {
        _logger = logger;
        _systemClock = systemClock;
        _listService = listService;
    }

    [HttpGet, Route("TopObservationsList")]
    public async Task<IActionResult> GetTopObservationsListAsync()
    {
        try
        {
            var username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                string errorMessage = "requesting username is null or empty";
                _logger.LogError(LoggingEvents.GetListNotFound, errorMessage);
                return StatusCode(500, errorMessage);
            }

            var viewModel = await _listService.GetTopObservationsAsync(username, _systemClock.GetToday.AddDays(-30));

            if (viewModel is null)
            {
                string errorMessage = "listService returned null";
                _logger.LogError(LoggingEvents.GetListNotFound, errorMessage);
                return StatusCode(500, errorMessage);
            }

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, "an exception was raised");
            return StatusCode(500, "an unexpected error occurred");
        }
    }

    [HttpGet, Route("life")]
    public async Task<IActionResult> GetLifeListAsync()
    {
        try
        {
            var username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                string errorMessage = "requesting username is null or empty";
                _logger.LogError(LoggingEvents.GetListNotFound, errorMessage);
                return StatusCode(500, errorMessage);
            }

            var viewModel = await _listService.GetLifeListsAsync(a => a.ApplicationUser.UserName == username);

            if (viewModel is null)
            {
                string errorMessage = "listService returned null";
                _logger.LogError(LoggingEvents.GetListNotFound, errorMessage);
                return StatusCode(500, errorMessage);
            }

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, "an exception was raised");
            return StatusCode(500, "an unexpected error occurred");
        }
    }
}