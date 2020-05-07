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
            _config.SetupGet(x => x[It.Is<string>(s => s == "Tokens:Issuer")]).Returns("http://localhost:55722");
        }

        [Fact]
        public async Task Login_ReturnsOkObjectResult_WithLoginDto()
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
        public async Task Login_ReturnsBadRequestObjectResult_WhenSignInResultIsNotSuccessful()
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
        public async Task Login_ReturnsBadRequestObjectResult_WhenSignInResultIsLockedOut()
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
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.IsType<AuthenticationResultDto>(objectResult.Value);

            var returnModel = objectResult.Value as AuthenticationResultDto;
            Assert.Equal(AuthenticationFailureReason.LockedOut, returnModel.FailureReason);
            Assert.Null(returnModel.AuthenticationToken);
        }

        [Fact]
        public async Task LoginWithEmailNotConfirmed_ReturnsBadRequest_WithEmailNotConfirmed()
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
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.IsType<AuthenticationResultDto>(objectResult.Value);

            var returnModel = objectResult.Value as AuthenticationResultDto;
            Assert.Equal(AuthenticationFailureReason.EmailConfirmationRequired, returnModel.FailureReason);
            Assert.Null(returnModel.AuthenticationToken);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenUserManagerReturnsNull()
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
            Assert.IsType<NotFoundObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is NotFoundObjectResult);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
            Assert.IsType<AuthenticationResultDto>(objectResult.Value);

            var returnModel = objectResult.Value as AuthenticationResultDto;
            Assert.Equal(AuthenticationFailureReason.Other, returnModel.FailureReason);
            Assert.Null(returnModel.AuthenticationToken);
        }

        [Fact]
        public async Task LoginWithInvalidModelState_ReturnsBadRequest_WithModelStateError()
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
        public async Task Login_ReturnsBadRequestResult_WhenExceptionIsRaised()
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
