using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;



namespace Birder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class NetworkController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INetworkRepository _networkRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public NetworkController(IMapper mapper
                                , IUnitOfWork unitOfWork
                                , ILogger<NetworkController> logger
                                , INetworkRepository networkRepository
                                , UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _networkRepository = networkRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetNetworkAsync()
        {
            try
            {
                var requestingUser = await _userManager.GetUserWithNetworkAsync(User.Identity.Name);

                if (requestingUser is null)
                {
                    _logger.LogError(LoggingEvents.GetItem, "User not found");
                    return NotFound("Requesting user not found");
                }

                var model = _mapper.Map<ApplicationUser, UserNetworkDto>(requestingUser);

                UserNetworkHelpers.SetupFollowersCollection(requestingUser, model.Followers);

                UserNetworkHelpers.SetupFollowingCollection(requestingUser, model.Following);

                return Ok(model);

            }
            catch(Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItem, ex, "GetMyNetworkAsync");
                return BadRequest("An unexpected error occurred");
            }
        }

        [HttpGet, Route("NetworkSuggestions")]
        public async Task<IActionResult> GetNetworkSuggestionsAsync()
        {
            try
            {
                var requestingUser = await _userManager.GetUserWithNetworkAsync(User.Identity.Name);

                if (requestingUser is null)
                {
                    _logger.LogError(LoggingEvents.GetItemNotFound, "The user was not found");
                    return NotFound("Requesting user not found");
                }

                var followersNotBeingFollowed = UserNetworkHelpers.GetFollowersNotBeingFollowedUserNames(requestingUser);

                if (followersNotBeingFollowed.Count() == 0)
                {
                    var followingUsernamesList = UserNetworkHelpers.GetFollowingUserNames(requestingUser.Following);
                    //var users = await _userManager.GetSuggestedBirdersToFollowAsync(requestingUser.UserName, followingUsernamesList);
                    var users = await _userManager.GetUsersAsync(users => !followingUsernamesList.Contains(users.UserName) && users.UserName != requestingUser.UserName);
                    return Ok(_mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<NetworkUserViewModel>>(users));
                }
                else
                {
                    //var users = await _userManager.GetFollowersNotFollowedAsync(followersNotBeingFollowed);
                    var users = await _userManager.GetUsersAsync(users => followersNotBeingFollowed.Contains(users.UserName));
                    return Ok(_mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<NetworkUserViewModel>>(users));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "Network");
                return BadRequest("An unexpected error occurred");
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

                var requestingUser = await _userManager.GetUserWithNetworkAsync(User.Identity.Name);

                if (requestingUser is null)
                {
                    _logger.LogError(LoggingEvents.GetItemNotFound, "The user was not found");
                    return NotFound("Requesting user not found");
                }

                var followingUsernamesList = UserNetworkHelpers.GetFollowingUserNames(requestingUser.Following);
                followingUsernamesList.Add(requestingUser.UserName);

                //var users = await _userManager.SearchBirdersToFollowAsync(searchCriterion, followingUsernamesList);
                var users = await _userManager.GetUsersAsync(users => users.NormalizedUserName.Contains(searchCriterion.ToUpper()) && !followingUsernamesList.Contains(users.UserName));
                return Ok(_mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<NetworkUserViewModel>>(users));
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetListNotFound, ex, "Network");
                return BadRequest("An unexpected error occurred");
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

                var requestingUser = await _userManager.GetUserWithNetworkAsync(User.Identity.Name);

                if (requestingUser is null)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, "Requesting user not found");
                    return NotFound("Requesting user not found");
                }

                var userToFollow = await _userManager.GetUserWithNetworkAsync(userToFollowDetails.UserName);

                if (userToFollow is null)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, "User to follow not found");
                    return NotFound("User to follow not found");
                }

                if (requestingUser == userToFollow)
                {
                    return BadRequest("Trying to follow yourself");
                }

                _networkRepository.Follow(requestingUser, userToFollow);

                await _unitOfWork.CompleteAsync();

                var viewModel = _mapper.Map<ApplicationUser, NetworkUserViewModel>(userToFollow);

                viewModel.IsFollowing = UserNetworkHelpers.UpdateIsFollowing(viewModel.UserName, requestingUser.Following);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Follow action error");
                return BadRequest(String.Format("An error occurred trying to follow user: {0}", userToFollowDetails.UserName));
            }
        }

        [HttpPost, Route("Unfollow")]
        public async Task<IActionResult> PostUnfollowUserAsync(NetworkUserViewModel userToUnfollowDetails) //, int currentPage)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, ModelStateErrorsExtensions.GetModelStateErrorMessages(ModelState));
                    return BadRequest("Invalid modelstate");
                }

                var requestingUser = await _userManager.GetUserWithNetworkAsync(User.Identity.Name);

                if (requestingUser is null)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, "Requesting user not found");
                    return NotFound("Requesting user not found");
                }

                var userToUnfollow = await _userManager.GetUserWithNetworkAsync(userToUnfollowDetails.UserName);

                if (userToUnfollow is null)
                {
                    _logger.LogError(LoggingEvents.UpdateItem, "User to Unfollow not found");
                    return NotFound("User to Unfollow not found");
                }

                if (requestingUser == userToUnfollow)
                {
                    return BadRequest("Trying to unfollow yourself");
                }

                _networkRepository.UnFollow(requestingUser, userToUnfollow);

                await _unitOfWork.CompleteAsync();

                var viewModel = _mapper.Map<ApplicationUser, NetworkUserViewModel>(userToUnfollow);

                viewModel.IsFollowing = UserNetworkHelpers.UpdateIsFollowing(viewModel.UserName, requestingUser.Following);

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Unfollow action error");
                return BadRequest($"An error occurred trying to unfollow user: {userToUnfollowDetails.UserName}");
            }
        }
    }
}