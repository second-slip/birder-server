using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class UserControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<UserController>> _logger;
        // private readonly IUnitOfWork _unitOfWork;
        // private readonly IUserRepository _userRepository;


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
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .Returns(Task.FromResult<ApplicationUser>(null));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetUserAsync(It.IsAny<string>());

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact] //Requesting own profile
        public async Task GetUserAsync_ReturnsOkObjectResult_WithOwnUserProfileViewModelObject()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ReturnsAsync(GetOwnUserProfile());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetUserAsync(It.IsAny<string>());

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
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ReturnsAsync(GetOtherMemberUserProfile());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetUserAsync(It.IsAny<string>());

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
        public async Task GetUserAsync_ReturnsBadRequest_WithModelStateError()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            //Add model error
            controller.ModelState.AddModelError("Test", "This is a test model error");

            // Act
            var result = await controller.GetUserAsync(It.IsAny<string>());

            var modelState = controller.ModelState;
            Assert.Equal(1, modelState.ErrorCount);
            Assert.True(modelState.ContainsKey("Test"));
            Assert.True(modelState["Test"].Errors.Count == 1);
            Assert.Equal("This is a test model error", modelState["Test"].Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task GetUserAsync_ReturnsBadRequestResult_WhenExceptionIsRaised()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
                 .ThrowsAsync(new InvalidOperationException());

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = GetTestClaimsPrincipal() }
            };

            // Act
            var result = await controller.GetUserAsync(It.IsAny<string>());

            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("There was an error getting the user", objectResult.Value);
        }

        #endregion


        #region Mock methods

        private ApplicationUser GetOwnUserProfile()
        {
            var user = new ApplicationUser()
            {
                UserName = "Own Profile Test"
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

        //ToDo: move to shared
        private ClaimsPrincipal GetTestClaimsPrincipal()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            return user;
        }

        #endregion
    }
}
