using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Helpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class UserControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<UserController>> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserControllerTests()
        {
            _userManager = SharedFunctions.InitialiseUserManager();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<UserController>>();

        }

        #region GetUser action tests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetUserProfileAsync_ReturnsBadRequest_WhenStringArgumentIsNullOrEmpty(string requestedUsername)
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();

            var controller = new UserController(_mapper, _logger.Object, mockUserManager.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal2("NonExistentUsername") }
            };

            // Act
            var result = await controller.GetUserProfileAsync(requestedUsername);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("An error occurred", objectResult.Value);
        }

        [Fact]
        public async Task GetUserProfileAsync_ReturnsNotFound_WhenRequestedUserIsNull()
        {
            // Arrange
            var controller = new UserController(_mapper, _logger.Object, _userManager);

            string requestedUsername = "This requested user does not exist";

            string requesterUsername = requestedUsername;

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal2(requesterUsername) }
            };

            // Act
            var result = await controller.GetUserProfileAsync(requestedUsername);

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("Requested user not found", objectResult.Value);
        }

        [Fact]
        public async Task GetUserProfileAsync_ReturnsOkObjectResultWithUserProfileViewModel_WhenRequestedUserIsRequestingUser()
        {
            // Arrange
            var controller = new UserController(_mapper, _logger.Object, _userManager);

            string requestedUsername = "Tenko";

            string requesterUsername = requestedUsername;

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal2(requesterUsername) }
            };

            // Act
            var result = await controller.GetUserProfileAsync(requestedUsername);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<OkObjectResult>(result);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<UserProfileViewModel>(objectResult.Value);

            var model = objectResult.Value as UserProfileViewModel;
            Assert.Equal(requestedUsername, model.UserName);
        }

        [Fact]
        public async Task GetUserProfileAsync_ReturnsNotFound_WhenRequesterUserIsNull()
        {
            // Arrange
            var controller = new UserController(_mapper, _logger.Object, _userManager);

            string requestedUsername = "Tenko";

            string requesterUsername = "This requested user does not exist";

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal2(requesterUsername) }
            };

            // Act
            var result = await controller.GetUserProfileAsync(requestedUsername);

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("Requesting user not found", objectResult.Value);
        }









        [Fact] //Requesting other member's profile
        public async Task GetUserProfileAsync_ReturnsOkObjectResult_WithOtherMembersUserProfileViewModelObject()
        {
            // Arrange
            //var mockUserManager = new Mock<UserManager<ApplicationUser>>();
            //mockUserManager.Setup(repo => repo.GetUserWithNetworkAsync(It.IsAny<string>()))
            //     .ReturnsAsync(GetOtherMemberUserProfile());


            var controller = new UserController(_mapper, _logger.Object, _userManager);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
            };

            string username = "example name"; // same as claims principle
                                              //new Claim(ClaimTypes.Name, "example name"),

            // Act
            var result = await controller.GetUserProfileAsync(username);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<UserProfileViewModel>(objectResult.Value);

            var model = objectResult.Value as UserProfileViewModel;
            Assert.Equal("Other Member's Profile Test", model.UserName);
        }



        #endregion




        #region Mock methods

        public ClaimsPrincipal GetTestClaimsPrincipal2(string username)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            return user;
        }

        private NetworkUserViewModel GetTestNetworkUserViewModel()
        {
            return new NetworkUserViewModel()
            {
                UserName = "Test Network View Model"
            };
        }

        private List<ApplicationUser> GetListOfApplicationUsers(int collectionLength)
        {
            var users = new List<ApplicationUser>();

            for (int i = 0; i < collectionLength; i++)
            {
                users.Add(new ApplicationUser()
                {
                    UserName = "Test User " + (i + 1).ToString()
                });
            }

            return users;
        }

        private ApplicationUser GetOwnUserProfile()
        {
            var user = new ApplicationUser()
            {
                UserName = "Own Profile Test",
                Followers = new List<Network>(),
                Following = new List<Network>()
            };

            return user;
        }

        private ApplicationUser GetOwnUserProfileWithOneFollower()
        {
            var user = new ApplicationUser()
            {
                UserName = "Own Profile Test With Follower",
                Followers = new List<Network>() { new Network() { ApplicationUser = new ApplicationUser(), Follower = new ApplicationUser() } },
                Following = new List<Network>()
            };

            return user;
        }

        private ApplicationUser GetOtherMemberUserProfile()
        {
            var user = new ApplicationUser()
            {
                UserName = "Other Member's Profile Test"
            };

            return user;
        }

        #endregion
    }
}
