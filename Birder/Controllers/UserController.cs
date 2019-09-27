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

        [HttpGet, Route("SearchNetwork")]
        public async Task<IActionResult> GetSearchNetworkAsync(string searchCriterion)
        {
            try
            {
                //?????
                if (string.IsNullOrEmpty(searchCriterion))
                {
                   // Log modelstate errors
                   return BadRequest("Hello");
                }

                var loggedinUser = await _userRepository.GetUserAndNetworkAsync(User.Identity.Name);

                if (loggedinUser == null)
                {
                    //log
                    return NotFound("User not found");
                }

                //ToDo: Guard?  Followers || Following == null ????????

                //ToDo: Move to own helper method
                //var followersUsernamesList = NetworkHelpers.GetFollowersUserNames(loggedinUser.Followers);
                var followingUsernamesList = NetworkHelpers.GetFollowingUserNames(loggedinUser.Following);
                followingUsernamesList.Add(loggedinUser.UserName);
                //IEnumerable<string> followersNotBeingFollowed = followersUsernamesList.Except(followingUsernamesList);

                var users = await _userRepository.SearchBirdersToFollowAsync(loggedinUser, searchCriterion, followingUsernamesList);
                return Ok(_mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<NetworkUserViewModel>>(users));
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "Network");
                return BadRequest();
            }
        }

        [HttpGet, Route("GetNetwork")]
        public async Task<IActionResult> GetNetworkAsync() //string searchCriterion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Log modelstate errors
                    return BadRequest("Hello");
                }

                //var username = User.Identity.Name;
                var loggedinUser = await _userRepository.GetUserAndNetworkAsync(User.Identity.Name);

                if(loggedinUser == null)
                {
                    //log
                    return NotFound("User not found");
                }


                //ToDo: Guard?  Followers || Following == null ????????
                //var followersUsernamesList = NetworkHelpers.GetFollowersUserNames(loggedinUser.Followers);

                //var followingUsernamesList = NetworkHelpers.GetFollowingUserNames(loggedinUser.Following);
                //followingUsernamesList.Add(loggedinUser.UserName);

                //IEnumerable<string> followersNotBeingFollowed = followersUsernamesList.Except(followingUsernamesList);

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
                return BadRequest();
                //return BadRequest(String.Format("An error occurred trying to follow user: {0}", userToFollowDetails.UserName));
            }
        }

        [HttpPost, Route("Follow")]
        public async Task<IActionResult> Follow(NetworkUserViewModel userToFollowDetails)
        {
            try
            {
                var loggedinUser = await _userRepository.GetUserAndNetworkAsync(User.Identity.Name);

                var userToFollow = await _userRepository.GetUserAndNetworkAsync(userToFollowDetails.UserName);

                if (loggedinUser == userToFollow)
                {
                    return BadRequest("Trying to follow yourself");
                }

                _userRepository.Follow(loggedinUser, userToFollow);
                await _unitOfWork.CompleteAsync();
                var viewModel = _mapper.Map<ApplicationUser, NetworkUserViewModel>(userToFollow);
                loggedinUser = await _userRepository.GetUserAndNetworkAsync(User.Identity.Name);
                viewModel.IsFollowing = loggedinUser.Following.Any(cus => cus.ApplicationUser.UserName == userToFollowDetails.UserName);
                return Ok(viewModel);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Follow action error");
                return BadRequest(String.Format("An error occurred trying to follow user: {0}", userToFollowDetails.UserName));
            }
        }

        [HttpPost, Route("Unfollow")]
        public async Task<IActionResult> Unfollow(NetworkUserViewModel userToFollowDetails) //, int currentPage)
        {
            try
            {
                var loggedinUser = await _userRepository.GetUserAndNetworkAsync(User.Identity.Name);
                var userToUnfollow = await _userRepository.GetUserAndNetworkAsync(userToFollowDetails.UserName);

                if (loggedinUser == userToUnfollow)
                {
                    return BadRequest("Trying to unfollow yourself");
                }

                _userRepository.UnFollow(loggedinUser, userToUnfollow);
                await _unitOfWork.CompleteAsync();
                var viewModel = _mapper.Map<ApplicationUser, NetworkUserViewModel>(userToUnfollow);
                loggedinUser = await _userRepository.GetUserAndNetworkAsync(User.Identity.Name);
                viewModel.IsFollowing = loggedinUser.Following.Any(cus => cus.ApplicationUser.UserName == userToFollowDetails.UserName);
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
