using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IUserRepository _userRepository;

        public UserController(IMapper mapper
                            , IUnitOfWork unitOfWork
                            , ILogger<UserController> logger
                            , IUserRepository userRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        [HttpGet, Route("GetUser")]
        public async Task<IActionResult> GetUserAsync(string username)
        {
            try
            {
                var loggedinUsername = User.Identity.Name;
                if (String.IsNullOrEmpty(username))
                {
                    username = loggedinUsername;
                }

                var user = await _userRepository.GetUserAndNetworkAsync(username);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var viewModel = _mapper.Map<ApplicationUser, UserProfileViewModel>(user);

                var loggedinUser = new ApplicationUser();

                if (string.Equals(loggedinUsername, username, StringComparison.InvariantCultureIgnoreCase))
                {
                    viewModel.IsOwnProfile = true;
                    loggedinUser = user;
                }
                else
                {
                    viewModel.IsFollowing = user.Followers.Any(cus => cus.Follower.UserName == loggedinUsername);
                    loggedinUser = await _userRepository.GetUserAndNetworkAsync(loggedinUsername);
                }

                UserProfileHelper.UpdateFollowingCollection(viewModel, loggedinUser, loggedinUsername);

                UserProfileHelper.UpdateFollowersCollection(viewModel, loggedinUser, loggedinUsername);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Follow action error");
                return BadRequest("There was an error getting the user");
            }
        }

        [HttpGet, Route("GetNetwork")]
        public async Task<IActionResult> GetNetworkAsync()
        {
            try
            {
                var loggedinUser = await _userRepository.GetUserAndNetworkAsync(User.Identity.Name);

                if (loggedinUser == null)
                {
                    _logger.LogError(LoggingEvents.GetItemNotFound, "The user was not found");
                    return NotFound("User not found");
                }

                //ToDo: Guard?  Followers || Following == null ????????
                var followersNotBeingFollowed = NetworkHelpers.GetFollowersNotBeingFollowedUserNames(loggedinUser);

                if (followersNotBeingFollowed.Count() == 0)
                {
                    var followingUsernamesList = NetworkHelpers.GetFollowingUserNames(loggedinUser.Following);
                    var users = await _userRepository.GetSuggestedBirdersToFollowAsync(loggedinUser, followingUsernamesList);
                    return Ok(_mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<NetworkUserViewModel>>(users));
                }
                else
                {
                    var users = await _userRepository.GetFollowersNotFollowedAsync(loggedinUser, followersNotBeingFollowed);
                    return Ok(_mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<NetworkUserViewModel>>(users));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "Network");
                return BadRequest("An error occurred");
            }
        }

        [HttpGet, Route("SearchNetwork")]
        public async Task<IActionResult> GetSearchNetworkAsync(string searchCriterion)
        {
            try
            {
                if (string.IsNullOrEmpty(searchCriterion))
                {
                    _logger.LogError(LoggingEvents.GetListNotFound, "The search criterion is null or empty");
                    return BadRequest("No search criterion");
                }

                var loggedinUser = await _userRepository.GetUserAndNetworkAsync(User.Identity.Name);

                if (loggedinUser == null)
                {
                    _logger.LogError(LoggingEvents.GetItemNotFound, "The user was not found");
                    return NotFound("User not found");
                }

                //ToDo: Guard?  Followers || Following == null ????????
                var followingUsernamesList = NetworkHelpers.GetFollowingUserNames(loggedinUser.Following);
                followingUsernamesList.Add(loggedinUser.UserName);

                var users = await _userRepository.SearchBirdersToFollowAsync(loggedinUser, searchCriterion, followingUsernamesList);
                return Ok(_mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<NetworkUserViewModel>>(users));
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "Network");
                return BadRequest("An error occurred");
            }
        }

        [HttpPost, Route("Follow")]
        public async Task<IActionResult> PostFollowUserAsync(NetworkUserViewModel userToFollowDetails)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return BadRequest("Invalid modelstate");
                }

                var loggedinUser = await _userRepository.GetUserAndNetworkAsync(User.Identity.Name);

                var userToFollow = await _userRepository.GetUserAndNetworkAsync(userToFollowDetails.UserName);

                if (loggedinUser == null || userToFollow == null)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, "User not found");
                    return NotFound("User not found");
                }

                if (loggedinUser == userToFollow)
                {
                    return BadRequest("Trying to follow yourself");
                }

                _userRepository.Follow(loggedinUser, userToFollow);

                await _unitOfWork.CompleteAsync();

                var viewModel = _mapper.Map<ApplicationUser, NetworkUserViewModel>(userToFollow);

                NetworkHelpers.UpdateIsFollowingInNetworkUserViewModel(viewModel, loggedinUser.Following);
                
                return Ok(viewModel);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Follow action error");
                return BadRequest(String.Format("An error occurred trying to follow user: {0}", userToFollowDetails.UserName));
            }
        }

        [HttpPost, Route("Unfollow")]
        public async Task<IActionResult> PostUnfollowUserAsync(NetworkUserViewModel userToFollowDetails) //, int currentPage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return BadRequest("Invalid modelstate");
                }

                var loggedinUser = await _userRepository.GetUserAndNetworkAsync(User.Identity.Name);
                var userToUnfollow = await _userRepository.GetUserAndNetworkAsync(userToFollowDetails.UserName);

                if (loggedinUser == null || userToUnfollow == null)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, "User not found");
                    return NotFound("User not found");
                }

                if (loggedinUser == userToUnfollow)
                {
                    return BadRequest("Trying to unfollow yourself");
                }

                _userRepository.UnFollow(loggedinUser, userToUnfollow);
                
                await _unitOfWork.CompleteAsync();
                
                var viewModel = _mapper.Map<ApplicationUser, NetworkUserViewModel>(userToUnfollow);

                NetworkHelpers.UpdateIsFollowingInNetworkUserViewModel(viewModel, loggedinUser.Following);
                
                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Unfollow action error");
                return BadRequest(String.Format("An error occurred trying to unfollow user: {0}", userToFollowDetails.UserName));
            }
        }
    }
}
