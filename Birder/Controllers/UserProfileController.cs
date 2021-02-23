using AutoMapper;
using Birder.Data.Model;
using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserProfileController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileController(IMapper mapper
                            , ILogger<UserProfileController> logger
                            , UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet] //, Route("GetUserProfile")]
        public async Task<IActionResult> GetUserProfileAsync(string requestedUsername)
        {
            // add properties for no. of species / observations
            // build new query object
            try
            {
                if (string.IsNullOrEmpty(requestedUsername))
                {
                    _logger.LogError(LoggingEvents.GetItem, "requestedUsername argument is null or empty at GetUserProfileAsync action");
                    return BadRequest("An error occurred");
                }

                var requestedUser = await _userManager.GetUserWithNetworkAsync(requestedUsername);

                if (requestedUser is null)
                {
                    _logger.LogError(LoggingEvents.GetItem, $"Username '{requestedUsername}' not found at GetUserProfileAsync action");
                    return NotFound("Requested user not found");
                }

                var requestedUserProfileViewModel = _mapper.Map<ApplicationUser, UserProfileViewModel>(requestedUser);

                var requesterUsername = User.Identity.Name;

                if (requesterUsername.Equals(requestedUsername))
                {
                    // Own profile requested...

                    // new method in Network repository?
                    requestedUserProfileViewModel.IsOwnProfile = true;

                    UserNetworkHelpers.SetupFollowingCollection(requestedUser, requestedUserProfileViewModel.Following);

                    UserNetworkHelpers.SetupFollowersCollection(requestedUser, requestedUserProfileViewModel.Followers);

                    return Ok(requestedUserProfileViewModel);
                }

                // Other user's profile requested...

                var requestingUser = await _userManager.GetUserWithNetworkAsync(requesterUsername);

                if (requestingUser is null)
                {
                    _logger.LogError(LoggingEvents.GetItem, $"Username '{requesterUsername}' not found at GetUserProfileAsync action");
                    return NotFound("Requesting user not found");
                }

                requestedUserProfileViewModel.IsFollowing = UserNetworkHelpers.UpdateIsFollowingProperty(requestingUser.UserName, requestedUser.Followers);

                UserNetworkHelpers.SetupFollowingCollection(requestingUser, requestedUserProfileViewModel.Following);

                UserNetworkHelpers.SetupFollowersCollection(requestingUser, requestedUserProfileViewModel.Followers);

                return Ok(requestedUserProfileViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Error at GetUserProfileAsync");
                return BadRequest("There was an error getting the user profile");
            }
        }
    }
}
