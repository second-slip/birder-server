using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Services;
using Birder.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class AccountControllerTests
    {
        private readonly Mock<ILogger<AccountController>> _logger;
        private readonly Mock<ISystemClockService> _systemClock;
        private readonly Mock<IEmailSender> _emailSender;
        private readonly Mock<IUrlService> _urlService;
        //private readonly UserManager<ApplicationUser> _userManager;

        public AccountControllerTests()
        {
            _logger = new Mock<ILogger<AccountController>>();
            _urlService = new Mock<IUrlService>();
            _emailSender = new Mock<IEmailSender>();
            _systemClock = new Mock<ISystemClockService>();
        }

        #region PostRegisterAsync unit tests

        [Fact]
        public async Task PostRegisterAsync_ReturnsBadRequestWithModelStateError_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();

            var testModel = new RegisterViewModel() { };

            var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

            controller.ModelState.AddModelError("Test", "This is a test model error");

            // Act
            var result = await controller.PostRegisterAsync(testModel);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            var expected = new SerializableError
                {
                    { "Test", new[] {"This is a test model error"}},
                };

            objectResult.Value.Should().BeOfType<SerializableError>();
            objectResult.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostRegisterAsync_ReturnsBadRequest_WhenExceptionRaised()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .ThrowsAsync(new InvalidOperationException());

            var testModel = new RegisterViewModel() { };

            var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

            // Act
            var result = await controller.PostRegisterAsync(testModel);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            var expected = "An error occurred";
            objectResult.Value.Should().BeOfType<string>();
            objectResult.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostRegisterAsync_ReturnsBadRequestWithModelState_WhenCreateUserIsUnsuccessful()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = string.Empty, Description = "Test IdentityError" })));

            var testModel = new RegisterViewModel() { };

            var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

            // Act
            var result = await controller.PostRegisterAsync(testModel);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            var expected = new SerializableError
                {
                    { string.Empty, new[] {"Test IdentityError"}},
                };

            objectResult.Value.Should().BeOfType<SerializableError>();
            objectResult.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostRegisterAsync_ReturnsOk_WhenCreateUserIsSuccessful()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .Returns(Task.FromResult(IdentityResult.Success));

            var testModel = new RegisterViewModel() { };

            var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

            // Act
            var result = await controller.PostRegisterAsync(testModel);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var expected = "New user successfully created";
            objectResult.Value.Should().BeOfType<string>();
            objectResult.Value.Should().BeEquivalentTo(expected);
        }

        #endregion


        #region ConfirmEmailAsync unit tests

        [Fact]
        public async Task ConfirmEmailAsync_ReturnsBadRequest_WhenUsernameArgumentIsNull()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();

            string testUsername = null;
            string testCode = string.Empty;

            var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

            // Act
            var result = await controller.ConfirmEmailAsync(testUsername, testCode);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            var expected = "An error occurred";
            objectResult.Value.Should().BeOfType<string>();
            objectResult.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ConfirmEmailAsync_ReturnsBadRequest_WhenCodeArgumentIsNull()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();

            string testUsername = string.Empty;
            string testCode = null;

            var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

            // Act
            var result = await controller.ConfirmEmailAsync(testUsername, testCode);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            var expected = "An error occurred";
            objectResult.Value.Should().BeOfType<string>();
            objectResult.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ConfirmEmailAsync_ReturnsNotFound_WhenRepositoryReturnsNull()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .Returns(Task.FromResult<ApplicationUser>(null));

            string testUsername = string.Empty;
            string testCode = string.Empty;

            var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

            // Act
            var result = await controller.ConfirmEmailAsync(testUsername, testCode);

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            var expected = "User not found";
            objectResult.Value.Should().BeOfType<string>();
            objectResult.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ConfirmEmailAsync_ReturnsBadRequest_WhenExceptionIsRaised()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ThrowsAsync(new InvalidOperationException());

            string testUsername = string.Empty;
            string testCode = string.Empty;

            var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

            // Act
            var result = await controller.ConfirmEmailAsync(testUsername, testCode);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            var expected = "An error occurred";
            objectResult.Value.Should().BeOfType<string>();
            objectResult.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ConfirmEmailAsync_BadRequest_WhenConfirmEmailAsyncFails()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(GetValidTestUser());
            mockUserManager.Setup(repo => repo.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = string.Empty, Description = "Test IdentityError" })));

            string testUsername = string.Empty;
            string testCode = string.Empty;

            var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

            // Act
            var result = await controller.ConfirmEmailAsync(testUsername, testCode);
            
            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            var expected = "An error occurred";
            objectResult.Value.Should().BeOfType<string>();
            objectResult.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task ConfirmEmailAsync_BadRequest_WhenConfirmEmailAsyncIsSuccessful()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(GetValidTestUser());
            mockUserManager.Setup(repo => repo.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .Returns(Task.FromResult(IdentityResult.Success));

            string testUsername = string.Empty;
            string testCode = string.Empty;

            var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

            // Act
            var result = await controller.ConfirmEmailAsync(testUsername, testCode);

            // Assert
            var objectResult = Assert.IsType<RedirectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is RedirectResult);

            var expected = "/confirmed-email";
            objectResult.Url.Should().BeOfType<string>();
            objectResult.Url.Should().BeEquivalentTo(expected);
        }



        #endregion


        private ApplicationUser GetValidTestUser()
        {
            var user = new ApplicationUser()
            {
                Email = "a@b.com",
                EmailConfirmed = true,
                UserName = "Test",
                Avatar = "",
                DefaultLocationLongitude = 3F,
                DefaultLocationLatitude = 3F
            };

            return user;
        }
    }
    
}
