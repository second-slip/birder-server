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
            for(int i = 0; i < viewModel.Followers.Count(); i++) 
            {
                viewModel.Followers.ElementAt(i).IsFollowing = user.Following.Any(cus => cus.ApplicationUser.UserName == viewModel.Followers.ElementAt(i).UserName);
                // item.IsFollowing = user.Following.Any(cus => cus.ApplicationUser.UserName == item.UserName);
            }

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
    }

    public class UserProfileViewModel
    {
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsOwnProfile { get; set; }
        public bool IsFollowing { get; set; }
        public IEnumerable<FollowerViewModel> Followers { get; set; }
        public IEnumerable<UserViewModel> Following { get; set; }
    }

    public class FollowerViewModel
    {
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public bool IsFollowing { get; set; }
    }


}
