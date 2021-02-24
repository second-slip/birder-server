using AutoMapper;
using Birder.Data.Model;
using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
            if (string.IsNullOrEmpty(requestedUsername))
            {
                _logger.LogError(LoggingEvents.GetItem, "requestedUsername argument is null or empty at GetUserProfileAsync action");
                return BadRequest("An error occurred");
            }

            try
            {
                var requestedUser = await _userManager.GetUserWithNetworkAsync(requestedUsername);

                if (requestedUser is null)
                {
                    _logger.LogError(LoggingEvents.GetItem, $"Username '{requestedUsername}' not found at GetUserProfileAsync action");
                    return NotFound("Requested user not found");
                }

                var requestedUserProfileViewModel = _mapper.Map<ApplicationUser, UserProfileViewModel>(requestedUser);

                requestedUserProfileViewModel.FollowersCount = requestedUser.Followers.Count();

                requestedUserProfileViewModel.FollowingCount = requestedUser.Following.Count();

                if (requestedUsername.Equals(User.Identity.Name))
                {
                    // Own profile requested...
                    requestedUserProfileViewModel.IsOwnProfile = true;
                }
                else
                {
                    // Other user's profile requested...
                    requestedUserProfileViewModel.IsFollowing = UserNetworkHelpers.UpdateIsFollowingProperty(User.Identity.Name, requestedUser.Followers);
                }

                return Ok(requestedUserProfileViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.Exception, ex, "Error at GetUserProfileAsync");
                return BadRequest("There was an error getting the user profile");
            }
        }
    }
}
