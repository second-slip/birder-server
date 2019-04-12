using AutoMapper;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.ViewModels;
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
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IMapper mapper
                            , ApplicationDbContext context
                            , IUserRepository userRepository
                            , UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [HttpGet, Route("GetUser")]
        public async Task<IActionResult> GetUser(string username)
        {
            // need defensive programming.  Username is url parameter
            var user = await _userRepository.GetUserAndNetworkAsyncByUserName(username);
            var viewModel = _mapper.Map<ApplicationUser, UserProfileViewModel>(user);

            // foreach (var item in viewModel.Followers)
            // for(int i = 0; i < viewModel.Followers.Count(); i++) 
            // {
            //     viewModel.Followers.ElementAt(i).IsFollowing = user.Following.Any(cus => cus.ApplicationUser.UserName == viewModel.Followers.ElementAt(i).UserName);
            //     // item.IsFollowing = user.Following.Any(cus => cus.ApplicationUser.UserName == item.UserName);
            // }

            var currentUser = User.Identity.Name;
            if (String.Equals(currentUser, username, StringComparison.InvariantCultureIgnoreCase))
            {
                viewModel.IsOwnProfile = true;
            }
            else
            {
                viewModel.IsFollowing = user.Following.Any(cus => cus.ApplicationUser.UserName == username);
            }

            return Ok(viewModel);
        }

        [HttpGet, Route("GetNetwork")]
        public async Task<IActionResult> GetNetwork(string searchCriterion)
        {
            var username = User.Identity.Name;
            var loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(username);
            // ApplicationUser loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(await _userAccessor.GetUser());
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
                //var username = User.Identity.Name;
                var loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(User.Identity.Name);
                // var loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(await _userAccessor.GetUser());
                var userToFollow = await _userRepository.GetUserAndNetworkAsyncByUserName(userToFollowDetails.UserName);

                if (loggedinUser == userToFollow)
                {
                    //return RedirectToAction("Details", new { userName = userName, page = currentPage });
                    return BadRequest("Trying to follow yourself");
                }
                else
                {
                    _userRepository.Follow(loggedinUser, userToFollow);
                    var viewModel = _mapper.Map<ApplicationUser, NetworkUserViewModel>(userToFollow);
                    loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(User.Identity.Name);
                    viewModel.IsFollowing = loggedinUser.Following.Any(cus => cus.ApplicationUser.UserName == userToFollowDetails.UserName);
                    return Ok(viewModel);
                    // return RedirectToAction("Details", new { userName = userName, page = currentPage });
                }
            }
            catch (Exception ex)
            {
                // _logger.LogError(LoggingEvents.GetItemNotFound, ex, "Follow action error");
                return BadRequest(String.Format("An error occurred trying to follow user: {0}", userToFollowDetails.UserName));
                //return RedirectToAction("Details", new { userName = userName, page = currentPage });
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //ToDo : Add validation
        [HttpPost, Route("Unfollow")]
        public async Task<IActionResult> Unfollow(NetworkUserViewModel userToFollowDetails) //, int currentPage)
        {
            //_logger.LogInformation(LoggingEvents.UpdateItem, "Unfollow action called");
            try
            {
                //var loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(await _userAccessor.GetUser());
                var loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(User.Identity.Name);
                var userToUnfollow = await _userRepository.GetUserAndNetworkAsyncByUserName(userToFollowDetails.UserName);

                if (loggedinUser == userToUnfollow)
                {
                    //return RedirectToAction("Details", new { userName = userName, page = currentPage });
                    return BadRequest("Trying to unfollow yourself");
                }
                else
                {
                    _userRepository.UnFollow(loggedinUser, userToUnfollow);
                    var viewModel = _mapper.Map<ApplicationUser, NetworkUserViewModel>(userToUnfollow);
                    loggedinUser = await _userRepository.GetUserAndNetworkAsyncByUserName(User.Identity.Name);
                    viewModel.IsFollowing = loggedinUser.Following.Any(cus => cus.ApplicationUser.UserName == userToFollowDetails.UserName);
                    return Ok(viewModel);
                    // return RedirectToAction("Details", new { userName = userName, page = currentPage });
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(LoggingEvents.GetItemNotFound, ex, "Unfollow action error");
                return BadRequest(String.Format("An error occurred trying to unfollow user: {0}", userToFollowDetails.UserName));
                //return RedirectToAction("Details", new { userName = userName, page = currentPage });
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
        // public IEnumerable<UserViewModel> Followers { get; set; }
        // public IEnumerable<UserViewModel> Following { get; set; }
    }

    public class NetworkUserViewModel
    {
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public bool IsFollowing { get; set; } = true;
    }


}
