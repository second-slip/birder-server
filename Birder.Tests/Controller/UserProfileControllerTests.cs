using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class UserProfileControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<UserProfileController>> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileControllerTests()
        {
            _userManager = SharedFunctions.InitialiseUserManager();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<UserProfileController>>();

        }

        #region GetUser action tests

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetUserProfileAsync_ReturnsBadRequest_WhenStringArgumentIsNullOrEmpty(string requestedUsername)
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();

            var controller = new UserProfileController(_mapper, _logger.Object, mockUserManager.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("NonExistentUsername") }
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
            var controller = new UserProfileController(_mapper, _logger.Object, _userManager);

            string requestedUsername = "This requested user does not exist";

            string requesterUsername = requestedUsername;

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
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
            var controller = new UserProfileController(_mapper, _logger.Object, _userManager);

            string requestedUsername = "Tenko";

            string requesterUsername = requestedUsername;

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
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
            var controller = new UserProfileController(_mapper, _logger.Object, _userManager);

            string requestedUsername = "Tenko";

            string requesterUsername = "This requested user does not exist";

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
            };

            // Act
            var result = await controller.GetUserProfileAsync(requestedUsername);

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<string>(objectResult.Value);
            Assert.Equal("Requesting user not found", objectResult.Value);
        }

        [Fact] 
        public async Task GetUserProfileAsync_ReturnsOkObjectResultWithUserProfileViewModel_WhenRequestedUserIsNotRequestingUser()
        {
            // Arrange
            var controller = new UserProfileController(_mapper, _logger.Object, _userManager);

            string requestedUsername = "Tenko";

            string requesterUsername = "Toucan";

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal(requesterUsername) }
            };

            // Act
            var result = await controller.GetUserProfileAsync(requestedUsername);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<UserProfileViewModel>(objectResult.Value);

            var model = objectResult.Value as UserProfileViewModel;
            Assert.Equal(requestedUsername, model.UserName);
        }

        #endregion

    }
}
