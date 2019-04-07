using AutoMapper;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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
            //var username = User.Identity.Name;
            //var user = await _userManager.FindByNameAsync(username);
            var user = await _userRepository.GetUserAndNetworkAsyncByUserName(username);
            var viewModel = _mapper.Map<ApplicationUser, UserProfileViewModel>(user);

            return Ok(viewModel);
        }
    }

    public class UserProfileViewModel
    {
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsFollowing { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        //public double DefaultLocationLatitude { get; set; }
        //public double DefaultLocationLongitude { get; set; }
    }
}
