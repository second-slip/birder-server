using AutoMapper;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
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
        private readonly IUserRepository _userRepository;

        public UserController(IMapper mapper
                            , IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet, Route("GetUser")]
        public async Task<IActionResult> GetUser(string username)
        {
            try
            {
                var loggedinUsername = User.Identity.Name;
                if (String.IsNullOrEmpty(username))
                {
                    username = loggedinUsername;
                }
                var user = await _userRepository.GetUserAndNetworkAsyncByUserName(username);
                if (user == null)
                {
                    return BadRequest("There was an error getting the user");
                }

                var viewModel = _mapper.Map<ApplicationUser, UserProfileViewModel>(user);

                var loggedinUser = new ApplicationUser();
                if (String.Equals(loggedinUsername, username, StringComparison.InvariantCultureIgnoreCase))
                {
                    viewModel.IsOwnProfile = true;
                    loggedinUser = user;
                }
                else
                {
                    viewModel.IsFollowing = user.Followers.Any(cus => cus.Follower.UserName == loggedinUsername);
                    loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(loggedinUsername);
                }

                // Check Following / Followers collections from the point of view of the loggedin user
                for (int i = 0; i < viewModel.Following.Count(); i++)
                {
                    viewModel.Following.ElementAt(i).IsFollowing = loggedinUser.Following.Any(cus => cus.ApplicationUser.UserName == viewModel.Following.ElementAt(i).UserName);
                    viewModel.Following.ElementAt(i).IsOwnProfile = viewModel.Following.ElementAt(i).UserName == loggedinUsername;
                }

                for (int i = 0; i < viewModel.Followers.Count(); i++)
                {
                    //viewModel.Followers.ElementAt(i).IsFollowing = loggedinUser.Followers.Any(cus => cus.Follower.UserName == viewModel.Followers.ElementAt(i).UserName);
                    viewModel.Followers.ElementAt(i).IsFollowing = loggedinUser.Following.Any(cus => cus.ApplicationUser.UserName == viewModel.Followers.ElementAt(i).UserName);
                    viewModel.Followers.ElementAt(i).IsOwnProfile = viewModel.Followers.ElementAt(i).UserName == loggedinUsername;
                }

                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                // _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Follow action error");
                return BadRequest("There was an error getting the user");
                //return RedirectToAction("Details", new { userName = userName, page = currentPage });
            }
        }

        [HttpGet, Route("GetNetwork")]
        public async Task<IActionResult> GetNetwork(string searchCriterion)
        {
            if (!ModelState.IsValid)
            {
                // Log modelstate errors
                return BadRequest(ModelState);
            }

            var username = User.Identity.Name;
            var loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(username);

            var viewModel = new List<NetworkUserViewModel>();

            if (String.IsNullOrEmpty(searchCriterion))
            {
                viewModel = _userRepository.GetSuggestedBirdersToFollow(loggedinUser);
                // followUserViewModel.SearchCriterion = searchCriterion;
            }
            else
            {
                viewModel = _userRepository.GetSuggestedBirdersToFollow(loggedinUser, searchCriterion);
                //followUserViewModel.SearchCriterion = searchCriterion;
            }
            return Ok(viewModel);
        }

        [HttpPost, Route("Follow")]
        public async Task<IActionResult> Follow(NetworkUserViewModel userToFollowDetails)
        {
            //_logger.LogInformation(LoggingEvents.UpdateItem, "Follow action called");
            try
            {
                var loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(User.Identity.Name);

                var userToFollow = await _userRepository.GetUserAndNetworkAsyncByUserName(userToFollowDetails.UserName);

                if (loggedinUser == userToFollow)
                {
                    return BadRequest("Trying to follow yourself");
                }

                _userRepository.Follow(loggedinUser, userToFollow);
                var viewModel = _mapper.Map<ApplicationUser, NetworkUserViewModel>(userToFollow);
                loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(User.Identity.Name);
                viewModel.IsFollowing = loggedinUser.Following.Any(cus => cus.ApplicationUser.UserName == userToFollowDetails.UserName);
                return Ok(viewModel);
                
            }
            catch (Exception ex)
            {
                // _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Follow action error");
                return BadRequest(String.Format("An error occurred trying to follow user: {0}", userToFollowDetails.UserName));
            }
        }

        [HttpPost, Route("Unfollow")]
        public async Task<IActionResult> Unfollow(NetworkUserViewModel userToFollowDetails) //, int currentPage)
        {
            //_logger.LogInformation(LoggingEvents.UpdateItem, "Unfollow action called");
            try
            {
                var loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(User.Identity.Name);
                var userToUnfollow = await _userRepository.GetUserAndNetworkAsyncByUserName(userToFollowDetails.UserName);

                if (loggedinUser == userToUnfollow)
                {
                    return BadRequest("Trying to unfollow yourself");
                }

                _userRepository.UnFollow(loggedinUser, userToUnfollow);
                var viewModel = _mapper.Map<ApplicationUser, NetworkUserViewModel>(userToUnfollow);
                loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(User.Identity.Name);
                viewModel.IsFollowing = loggedinUser.Following.Any(cus => cus.ApplicationUser.UserName == userToFollowDetails.UserName);
                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                //_logger.LogError(LoggingEvents.GetItemNotFound, ex, "Unfollow action error");
                return BadRequest(String.Format("An error occurred trying to unfollow user: {0}", userToFollowDetails.UserName));
            }
        }
    }

    public class UserProfileViewModel
    {
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsOwnProfile { get; set; }
        public bool IsFollowing { get; set; }
        public IEnumerable<FollowerViewModel> Followers { get; set; }
        public IEnumerable<FollowingViewModel> Following { get; set; }
    }

    public class NetworkUserViewModel
    {
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public bool IsFollowing { get; set; }
        public bool IsOwnProfile { get; set; }
    }

    public class FollowingViewModel : NetworkUserViewModel
    {
    }

    public class FollowerViewModel : NetworkUserViewModel
    {
    }
}
