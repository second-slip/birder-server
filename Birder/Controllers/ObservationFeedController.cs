namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ObservationFeedController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IObservationQueryService _observationQueryService;

    public ObservationFeedController(ILogger<ObservationFeedController> logger
                                   , UserManager<ApplicationUser> userManager
                                   , IObservationQueryService observationQueryService)
    {
        _logger = logger;
        _userManager = userManager;
        _observationQueryService = observationQueryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPublicFeedAsync(int pageIndex, int pageSize)
    {
        try
        {
            var publicObservations = await _observationQueryService.GetPagedObservationsFeedAsync(pl => pl.SelectedPrivacyLevel == PrivacyLevel.Public, pageIndex, pageSize);

            if (publicObservations is null)
            {
                _logger.LogWarning(LoggingEvents.GetListNotFound, "observations list is null");
                return StatusCode(500, "an unexpected error occurred");
            }

            return Ok(publicObservations);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the observations feed");
            return StatusCode(500, "an unexpected error occurred");
        }
    }

    [HttpGet, Route("network")]
    public async Task<IActionResult> GetNetworkFeedAsync(int pageIndex, int pageSize) //, ObservationFeedFilter filter)
    {
        try
        {
            var requestingUserAndNetwork = await _userManager.GetUserWithNetworkAsync(User.Identity.Name);

            if (requestingUserAndNetwork is null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Requesting user not found");
                return StatusCode(500, "requesting user not found");
            }

            var followingUsernamesList = UserNetworkHelpers.GetFollowingUserNames(requestingUserAndNetwork.Following);

            followingUsernamesList.Add(requestingUserAndNetwork.UserName);

            var networkObservations = await _observationQueryService.GetPagedObservationsFeedAsync(o => followingUsernamesList.Contains(o.ApplicationUser.UserName), pageIndex, pageSize);

            if (networkObservations is null)
            {
                _logger.LogWarning(LoggingEvents.GetListNotFound, "network observations list is null");
                return StatusCode(500, "an unexpected error occurred");
            }

            return Ok(networkObservations);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the observations feed");
            return StatusCode(500, "an unexpected error occurred");
        }
    }

    [HttpGet, Route("user")]
    public async Task<IActionResult> GetUserFeedAsync(int pageIndex, int pageSize)
    {
        try
        {
            var username = User.Identity.Name; //  info: removed null check as should be authenticated & authorised

            var userObservations = await _observationQueryService.GetPagedObservationsFeedAsync(o => o.ApplicationUser.UserName == username, pageIndex, pageSize);

            if (userObservations is null)
            {
                _logger.LogWarning(LoggingEvents.GetListNotFound, "User Observations list was null at GetObservationsFeedAsync()");
                return StatusCode(500, "an unexpected error occurred");
            }

            return Ok(userObservations);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.GetListNotFound, ex, "An error occurred getting the observations feed");
            return StatusCode(500, "an unexpected error occurred");
        }
    }
}