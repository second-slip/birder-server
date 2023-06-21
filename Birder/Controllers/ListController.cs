namespace Birder.Controllers;

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

    [HttpGet, Route("top")]
    public async Task<IActionResult> GetTopObservationsListAsync()
    {
        try
        {
            var username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                _logger.LogError(LoggingEvents.GetListNotFound, "requesting username is null or empty");
                return BadRequest();
            }

            var viewModel = await _listService.GetTopObservationsAsync(username, _systemClock.GetToday.AddDays(-30));

            if (viewModel is null)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, "listService returned null");
                return StatusCode(500, "an unexpected error occurred");
            }

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, ex.Message);
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
                _logger.LogError(LoggingEvents.GetListNotFound, "requesting username is null or empty");
                return BadRequest();
            }

            var viewModel = await _listService.GetLifeListAsync(a => a.ApplicationUser.UserName == username);

            if (viewModel is null)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, "listService returned null");
                return StatusCode(500, "an unexpected error occurred");
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