using AutoMapper;
using Birder.Data.Model;

namespace Birder.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class UserProfileController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IObservationsAnalysisService _observationsAnalysisService;

    public UserProfileController(IMapper mapper
                        , ILogger<UserProfileController> logger
                        , UserManager<ApplicationUser> userManager
                        , IObservationsAnalysisService observationsAnalysisService)
    {
        _mapper = mapper;
        _logger = logger;
        _userManager = userManager;
        _observationsAnalysisService = observationsAnalysisService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserProfileAsync(string requestedUsername)
    {
        // ToDo: make new query object
        if (string.IsNullOrEmpty(requestedUsername))
        {
            _logger.LogError(LoggingEvents.GetItem, "requestedUsername argument is null or empty at GetUserProfileAsync action");
            return BadRequest($"{nameof(requestedUsername)} argument is null or empty");
        }

        try
        {
            var requestedUser = await _userManager.GetUserWithNetworkAsync(requestedUsername);

            if (requestedUser is null)
            {
                _logger.LogError(LoggingEvents.GetItem, $"Username '{requestedUsername}' not found at GetUserProfileAsync action");
                return StatusCode(500, "userManager returned null");
            }

            var requestedUserProfileViewModel = _mapper.Map<ApplicationUser, UserProfileViewModel>(requestedUser);

            requestedUserProfileViewModel.FollowersCount = requestedUser.Followers.Count;

            requestedUserProfileViewModel.FollowingCount = requestedUser.Following.Count;

            if (requestedUsername.Equals(User.Identity.Name))
            {
                // Own profile requested...
                requestedUserProfileViewModel.IsOwnProfile = true;
            }
            else
            {
                // Other user's profile requested...
                requestedUserProfileViewModel.IsFollowing = UserNetworkHelpers.UpdateIsFollowingProperty(User.Identity.Name, requestedUser.Followers);
                requestedUserProfileViewModel.ObservationCount = await _observationsAnalysisService.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == requestedUsername);
            }

            //return Ok(requestedUser);
            return Ok(requestedUserProfileViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.Exception, ex, "Error at GetUserProfileAsync");
            return StatusCode(500, "There was an error getting the user profile");
        }
    }
}