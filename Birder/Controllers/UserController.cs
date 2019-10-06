using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INetworkRepository _networkRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IMapper mapper
                            , IUnitOfWork unitOfWork
                            , ILogger<UserController> logger
                            , INetworkRepository networkRepository
                            , UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _networkRepository = networkRepository;
        }

        [HttpGet, Route("GetUserProfile")]
        public async Task<IActionResult> GetUserProfileAsync(string requestedUsername)
        {
            if (string.IsNullOrEmpty(requestedUsername))
            {
                //Bad Request
                return BadRequest();
            }

            var requestedUser = await _userManager.GetUserWithNetworkAsync(requestedUsername);

            if (requestedUser == null)
            {
                return NotFound("User not found");
            }

            var requestedUserProfileViewModel = _mapper.Map<ApplicationUser, UserProfileViewModel>(requestedUser);

            var requesterUsername = User.Identity.Name;

            if(requesterUsername.Equals(requestedUsername))
            {
                // Own profile requested
                requestedUserProfileViewModel.IsOwnProfile = true;
                
                UserProfileHelper.UpdateFollowingCollection(requestedUserProfileViewModel, requestedUser); //, loggedinUsername);

                UserProfileHelper.UpdateFollowersCollection(requestedUserProfileViewModel, requestedUser);

                return Ok(requestedUserProfileViewModel);
            }

            var requestingUser = await _userManager.GetUserWithNetworkAsync(requesterUsername);

            if (requestingUser == null)
            {
                return NotFound("Requesting user not found");
            }

            UserProfileHelper.UpdateIsFollowingProperty(requestedUserProfileViewModel, requestedUser, requestingUser);

            UserProfileHelper.UpdateFollowingCollection(requestedUserProfileViewModel, requestingUser); //, loggedinUsername);

            UserProfileHelper.UpdateFollowersCollection(requestedUserProfileViewModel, requestingUser);

            return Ok(requestedUserProfileViewModel);
        }

        //[HttpGet, Route("GetUser")]
        //public async Task<IActionResult> GetUserAsync(string username) //requestedUsername
        //{
        //    try
        //    {
        //        //var requestingUser = User.Identity.Name;
        //        var loggedinUsername = User.Identity.Name;
        //        if (String.IsNullOrEmpty(username))
        //        {
        //            username = loggedinUsername;
        //        }

        //        var user = await _userManager.GetUserWithNetworkAsync(username);
                
        //        if (user == null)
        //        {
        //            return NotFound("User not found");
        //        }

        //        //requestedProfileViewModel
        //        var viewModel = _mapper.Map<ApplicationUser, UserProfileViewModel>(user);

        //        //NAME IS TERRIBLE: EITHER THE LOGGED IN USER OR <<REQUESTING USER>>
        //        //var requestedUser
        //        var loggedinUser = new ApplicationUser();

        //        if (string.Equals(loggedinUsername, username, StringComparison.InvariantCultureIgnoreCase))
        //        {
        //            viewModel.IsOwnProfile = true;
        //            loggedinUser = user;
        //        }
        //        else
        //        {
        //            viewModel.IsFollowing = user.Followers.Any(cus => cus.Follower.UserName == loggedinUsername);
        //            // NULL check
        //            loggedinUser = await _userManager.GetUserWithNetworkAsync(loggedinUsername);
        //            // NULL check
        //        }

        //        UserProfileHelper.UpdateFollowingCollection(viewModel, loggedinUser); //, loggedinUsername);

        //        UserProfileHelper.UpdateFollowersCollection(viewModel, loggedinUser); //, loggedinUsername);

        //        return Ok(viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Follow action error");
        //        return BadRequest("There was an error getting the user");
        //    }
        //}
    }
}
