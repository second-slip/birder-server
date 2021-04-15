using Birder.Controllers;
using Birder.Data.Model;
using Birder.Services;
using Birder.TestsHelpers;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<ILogger<AuthenticationController>> _logger;
        private readonly Mock<IConfiguration> _config;
        private readonly ISystemClockService _systemClock;

        public AuthenticationControllerTests()
        {
            _logger = new Mock<ILogger<AuthenticationController>>();
            _systemClock = new SystemClockService();
            _config = new Mock<IConfiguration>();
            _config.SetupGet(x => x[It.Is<string>(s => s == "Scheme")]).Returns("https://");
            _config.SetupGet(x => x[It.Is<string>(s => s == "Domain")]).Returns("localhost:55722");
            _config.SetupGet(x => x[It.Is<string>(s => s == "FlickrApiKey")]).Returns("ggjh");
            _config.SetupGet(x => x[It.Is<string>(s => s == "MapApiKey")]).Returns("fjfgjn");
            _config.SetupGet(x => x[It.Is<string>(s => s == "TokenKey")]).Returns("fjfgdfdfeTTjn3wq");
        }

        [Fact]
        public async Task Returns_OkObjectResult_With_Dto()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                        .ReturnsAsync(GetValidTestUser());

            var mockSignInManager = SharedFunctions.InitialiseMockSignInManager(mockUserManager);
            mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false))
                            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Success));

            var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, _logger.Object,
                                                           _systemClock, _config.Object);

            var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

            // Act
            var result = await controller.Login(model);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.IsType<AuthenticationResultDto>(objectResult.Value);
            var returnModel = objectResult.Value as AuthenticationResultDto;
            Assert.Equal(AuthenticationFailureReason.None, returnModel.FailureReason);
            Assert.NotNull(returnModel.AuthenticationToken);
        }

        [Fact]
        public async Task Returns_500_And_Other_Status_When_Login_Is_Unsuccessful_For_Any_Other_Reason()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                        .ReturnsAsync(GetValidTestUser());

            var mockSignInManager = SharedFunctions.InitialiseMockSignInManager(mockUserManager);
            mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false))
                            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.Failed));

            var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, _logger.Object,
                                                           _systemClock, _config.Object);

            var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

            // Act
            var result = await controller.Login(model);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is ObjectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.IsType<AuthenticationResultDto>(objectResult.Value);

            var returnModel = objectResult.Value as AuthenticationResultDto;
            Assert.Equal(AuthenticationFailureReason.Other, returnModel.FailureReason);
            Assert.Null(returnModel.AuthenticationToken);
        }

        [Fact]
        public async Task Returns_500_And_LockedOut_Status_When_User_Is_Locked_Out()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                        .ReturnsAsync(GetValidTestUser());

            var mockSignInManager = SharedFunctions.InitialiseMockSignInManager(mockUserManager);
            mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false))
                            .Returns(Task.FromResult(Microsoft.AspNetCore.Identity.SignInResult.LockedOut));

            var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, _logger.Object,
                                                           _systemClock, _config.Object);

            var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

            // Act
            var result = await controller.Login(model);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is ObjectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.IsType<AuthenticationResultDto>(objectResult.Value);

            var returnModel = objectResult.Value as AuthenticationResultDto;
            Assert.Equal(AuthenticationFailureReason.LockedOut, returnModel.FailureReason);
            Assert.Null(returnModel.AuthenticationToken);
        }

        [Fact]
        public async Task Returns_500_And_EmailConfirmationRequired_Status_When_User_Email_Not_Confirmed()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                            .ReturnsAsync(GetTestUserWithEmailNotConfirmed());

            var mockSignInManager = SharedFunctions.InitialiseMockSignInManager(mockUserManager);

            var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, _logger.Object,
                                                           _systemClock, _config.Object);
            
            var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

            // Act
            var result = await controller.Login(model);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is ObjectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.IsType<AuthenticationResultDto>(objectResult.Value);

            var returnModel = objectResult.Value as AuthenticationResultDto;
            Assert.Equal(AuthenticationFailureReason.EmailConfirmationRequired, returnModel.FailureReason);
            Assert.Null(returnModel.AuthenticationToken);
        }

        [Fact]
        public async Task Returns_500_And_Other_Status_When_User_Is_Null()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                            .Returns(Task.FromResult<ApplicationUser>(null));

            var mockSignInManager = SharedFunctions.InitialiseMockSignInManager(mockUserManager);

            var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, _logger.Object,
                                                           _systemClock, _config.Object);

            var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

            // Act
            var result = await controller.Login(model);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is ObjectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.IsType<AuthenticationResultDto>(objectResult.Value);

            var returnModel = objectResult.Value as AuthenticationResultDto;
            Assert.Equal(AuthenticationFailureReason.Other, returnModel.FailureReason);
            Assert.Null(returnModel.AuthenticationToken);
        }

        [Fact]
        public async Task Returns_500_And_Other_Status_When_ModelState_Is_Invalid()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();

            var mockSignInManager = SharedFunctions.InitialiseMockSignInManager(mockUserManager);
            
            var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, _logger.Object, _systemClock, _config.Object);

            //Add model error
            controller.ModelState.AddModelError("Test", "This is a test model error");

            var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

            // Act
            var result = await controller.Login(model);

            // Assert
            var modelState = controller.ModelState;
            Assert.Equal(1, modelState.ErrorCount);
            Assert.True(modelState.ContainsKey("Test"));
            Assert.True(modelState["Test"].Errors.Count == 1);
            Assert.Equal("This is a test model error", modelState["Test"].Errors[0].ErrorMessage);

            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.IsType<AuthenticationResultDto>(objectResult.Value);

            var returnModel = objectResult.Value as AuthenticationResultDto;
            Assert.Equal(AuthenticationFailureReason.Other, returnModel.FailureReason);
            Assert.Null(returnModel.AuthenticationToken);
        }

        [Fact]
        public async Task Returns_500_And_Exception_Status_When_Exception_Is_Raised()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException());

            var mockSignInManager = SharedFunctions.InitialiseMockSignInManager(mockUserManager);

            var controller = new AuthenticationController(mockUserManager.Object, mockSignInManager.Object, _logger.Object, _systemClock, _config.Object);

            var model = new LoginViewModel() { UserName = "", Password = "", RememberMe = false };

            // Act
            var result = await controller.Login(model);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is ObjectResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.IsType<AuthenticationResultDto>(objectResult.Value);

            var returnModel = objectResult.Value as AuthenticationResultDto;
            Assert.Equal(AuthenticationFailureReason.Other, returnModel.FailureReason);
            Assert.Null(returnModel.AuthenticationToken);
        }



        #region Mock methods



        private ApplicationUser GetTestUserWithEmailNotConfirmed()
        {
            var user = new ApplicationUser()
            {
                EmailConfirmed = false,
            };

            return user;
        }

        private ApplicationUser GetValidTestUser()
        {
            var user = new ApplicationUser()
            {
                Email = "a@b.com",
                EmailConfirmed = true,
                UserName = "Test",
                Avatar = ""
            };

            return user;
        }

        #endregion
    }
}
