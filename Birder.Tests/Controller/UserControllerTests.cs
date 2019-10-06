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

        public UserControllerTests()
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<UserController>>();

        }

        #region GetUser action tests

        [Fact]
        public async Task GetUserAsync_ReturnsBadRequest_WhenRepositoryReturnsNull()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseUserManager();

            var t = await mockUserManager.GetUserWithNetworkAsync("Tenko");
            //.Returns(Task.FromResult<ApplicationUser>(null));

            var controller = new UserController(_mapper, _logger.Object, mockUserManager);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal2("NonExistentUsername") }
            };

            string username = "Tenko";

            // Act
            var result = await controller.GetUserProfileAsync(username);

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("User not found", objectResult.Value);
        }

        [Fact] //Requesting own profile
        public async Task GetUserAsync_ReturnsOkObjectResult_WithOwnUserProfileViewModelObject()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<ApplicationUser>>();
            mockUserManager.Setup(repo => repo.GetUserWithNetworkAsync(It.IsAny<string>()))
                 .ReturnsAsync(GetOwnUserProfile());


            var controller = new UserController(_mapper, _logger.Object, mockUserManager.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
            };

            string username = "Test username";

            // Act
            var result = await controller.GetUserProfileAsync(username);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<OkObjectResult>(result);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<UserProfileViewModel>(objectResult.Value);

            var model = objectResult.Value as UserProfileViewModel;
            Assert.Equal("Own Profile Test", model.UserName);
        }

        [Fact] //Requesting other member's profile
        public async Task GetUserAsync_ReturnsOkObjectResult_WithOtherMembersUserProfileViewModelObject()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<ApplicationUser>>();
            mockUserManager.Setup(repo => repo.GetUserWithNetworkAsync(It.IsAny<string>()))
                 .ReturnsAsync(GetOtherMemberUserProfile());


            var controller = new UserController(_mapper, _logger.Object, mockUserManager.Object);
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

        [Fact]
        public async Task GetUserAsync_ReturnsBadRequestResult_WhenExceptionIsRaised()
        {
            // Arrange
            var mockUserManager = new Mock<UserManager<ApplicationUser>>();
            mockUserManager.Setup(repo => repo.GetUserWithNetworkAsync(It.IsAny<string>()))
                 .ThrowsAsync(new InvalidOperationException());

            var controller = new UserController(_mapper, _logger.Object, mockUserManager.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
            };

            string username = "Test username";

            // Act
            var result = await controller.GetUserProfileAsync(username);

            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("There was an error getting the user", objectResult.Value);
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
