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
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    //***************************
    // ToDo: Add logging
    //***************************
    public class ManageControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUrlService> _urlService;
        private readonly Mock<IEmailSender> _emailSender;
        private readonly Mock<ILogger<ManageController>> _logger;
        private readonly Mock<IFileClient> _fileClient;

        public ManageControllerTests()
        {
            _fileClient = new Mock<IFileClient>();
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BirderMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
            _logger = new Mock<ILogger<ManageController>>();
            _urlService = new Mock<IUrlService>();
            _emailSender = new Mock<IEmailSender>();
        }

        #region GetUserProfileAsync unit tests

        [Fact]
        public async Task GetUserProfileAsync_ReturnsNotFound_WhenUserIsNull()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .Returns(Task.FromResult<ApplicationUser>(null));

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            // Act
            var result = await controller.GetUserProfileAsync();

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("User not found", objectResult.Value);
        }

        [Fact]
        public async Task GetUserProfileAsync_ReturnsBadRequestWithStringObject_WhenExceptionIsRaised()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ThrowsAsync(new InvalidOperationException());

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            // Act
            var result = await controller.GetUserProfileAsync();

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("There was an error getting the user", objectResult.Value);
        }

        [Fact]
        public async Task GetUserProfileAsync_ReturnsOkWithManageProfileViewModel_WhenUserIsFound()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(GetValidTestUser());

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            // Act
            var result = await controller.GetUserProfileAsync();

            // Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.IsType<OkObjectResult>(result);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var returnModel = Assert.IsType<ManageProfileViewModel>(objectResult.Value);
            Assert.Equal("Test", returnModel.UserName);
        }

        #endregion


        #region UpdateProfileAsync unit tests

        [Fact]
        public async Task UpdateProfileAsync_ReturnsBadRequest_WithModelStateError()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            //mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
            //               .ReturnsAsync(GetValidTestUser());

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            //Add model error
            controller.ModelState.AddModelError("Test", "This is a test model error");

            var model = new ManageProfileViewModel() { UserName = "", IsEmailConfirmed = true };

            // Act
            var result = await controller.UpdateProfileAsync(model);

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
            //Assert.IsType<String>(objectResult.Value);

            // Assert
            var returnError = Assert.IsType<SerializableError>(objectResult.Value);
            Assert.Single(returnError); //Assert.Equal(2, returnError.Count);
            Assert.True(returnError.ContainsKey("Test"));

            var values = returnError["Test"] as String[];
            Assert.True(values[0] == "This is a test model error");


            var expected = new SerializableError
                {
                    { "Test", new[] {"This is a test model error"}},
                };

            objectResult.Value.Should().BeOfType<SerializableError>();
            objectResult.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateProfileAsync_ReturnsNotFound_WithUserManagerReturnsNull()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .Returns(Task.FromResult<ApplicationUser>(null));

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new ManageProfileViewModel() { UserName = "", IsEmailConfirmed = true };

            // Act
            var result = await controller.UpdateProfileAsync(model);

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("User not found", objectResult.Value);
        }

        [Fact]
        public async Task UpdateProfileAsync_ReturnsBadRequestWithStringObject_WhenExceptionIsRaised()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ThrowsAsync(new InvalidOperationException());

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new ManageProfileViewModel() { UserName = "", IsEmailConfirmed = true };

            // Act
            var result = await controller.UpdateProfileAsync(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("There was an error updating the user", objectResult.Value);
        }

        [Fact]
        public async Task UpdateProfileAsync_ReturnsBadRequestWithModelState_WhenUsernameIsAlreadyInUse()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(GetValidTestUser());

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            // model username IS the same as GetValidTestUser username
            var model = new ManageProfileViewModel() { UserName = "Test User", IsEmailConfirmed = true };

            // Act
            var result = await controller.UpdateProfileAsync(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            //Assert.IsType<String>(objectResult.Value);

            // Assert
            var returnError = Assert.IsType<SerializableError>(objectResult.Value);
            Assert.Single(returnError); //Assert.Equal(2, returnError.Count);
            Assert.True(returnError.ContainsKey("Username"));

            var values = returnError["Username"] as String[];
            Assert.True(values[0] == $"Username '{model.UserName}' is already taken.");
        }

        [Fact]
        public async Task UpdateProfileAsync_ReturnsBadRequestWithModelState_WhenSetUserNameResultNotSucceeded()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var user = GetValidTestUser();
            mockUserManager.SetupSequence(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(user)
                           .Returns(Task.FromResult<ApplicationUser>(null));
            mockUserManager.Setup(repo => repo.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .Returns(Task.FromResult(IdentityResult.Failed()));

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new ManageProfileViewModel() { UserName = "Test User", IsEmailConfirmed = true };

            // Act
            var result = await controller.UpdateProfileAsync(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            var returnError = Assert.IsType<SerializableError>(objectResult.Value);
            Assert.Single(returnError); //Assert.Equal(2, returnError.Count);
            Assert.True(returnError.ContainsKey("Username"));

            var values = returnError["Username"] as String[];
            Assert.True(values[0] == $"Unexpected error occurred setting username for user with ID '{user.Id}'.");
        }


        [Fact]
        public async Task UpdateProfileAsync_ReturnsBadRequestWithModelState_WhenSetEmailAysncResultNotSucceeded()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            var user = GetValidTestUser();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(user);
            //.Returns(Task.FromResult<ApplicationUser>(null));
            mockUserManager.Setup(repo => repo.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .Returns(Task.FromResult(IdentityResult.Failed()));

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new ManageProfileViewModel() { UserName = "Test", Email = "", IsEmailConfirmed = true };

            // Act
            var result = await controller.UpdateProfileAsync(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

            var returnError = Assert.IsType<SerializableError>(objectResult.Value);
            Assert.Single(returnError); //Assert.Equal(2, returnError.Count);
            Assert.True(returnError.ContainsKey("Email"));

            var values = returnError["Email"] as String[];
            Assert.True(values[0] == $"Unexpected error occurred setting email for user with ID '{user.Id}'.");
        }

        [Fact]
        public async Task UpdateProfileAsync_ReturnsBadRequest_WhenUpdateNotSuccessful()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.SetupSequence(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(GetValidTestUser)
                           .Returns(Task.FromResult<ApplicationUser>(null));
            mockUserManager.Setup(repo => repo.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .Returns(Task.FromResult(IdentityResult.Success));
            mockUserManager.Setup(repo => repo.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .Returns(Task.FromResult(IdentityResult.Success));
            mockUserManager.Setup(repo => repo.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
                           .Returns(Task.FromResult(It.IsAny<string>()));
            mockUserManager.Setup(repo => repo.UpdateAsync(It.IsAny<ApplicationUser>()))
                           .Returns(Task.FromResult(IdentityResult.Failed()));

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new ManageProfileViewModel() { UserName = "Test User", Email = "", IsEmailConfirmed = true };

            // Act
            var result = await controller.UpdateProfileAsync(model);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            var returnObject = Assert.IsType<string>(objectResult.Value);
            Assert.Equal("There was an error updating the user", returnObject);
        }

        [Fact]
        public async Task UpdateProfileAsync_ReturnsOkObject_WhenSuccessful()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.SetupSequence(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(GetValidTestUser)
                           .Returns(Task.FromResult<ApplicationUser>(null));
            mockUserManager.Setup(repo => repo.SetUserNameAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .Returns(Task.FromResult(IdentityResult.Success));
            mockUserManager.Setup(repo => repo.SetEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                           .Returns(Task.FromResult(IdentityResult.Success));
            mockUserManager.Setup(repo => repo.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()))
                           .Returns(Task.FromResult(It.IsAny<string>()));
            mockUserManager.Setup(repo => repo.UpdateAsync(It.IsAny<ApplicationUser>()))
                           .Returns(Task.FromResult(IdentityResult.Success));

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new ManageProfileViewModel() { UserName = "Test User", Email = "", IsEmailConfirmed = true };

            // Act
            var result = await controller.UpdateProfileAsync(model);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var returnObject = Assert.IsType<ManageProfileViewModel>(objectResult.Value);
            Assert.Equal(model, returnObject);
        }

        #endregion


        #region SetLocationAsync unit tests

        [Fact]
        public async Task SetLocationAsync_ReturnsBadRequest_WithModelStateError()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            //mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
            //               .ReturnsAsync(GetValidTestUser());

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            //Add model error
            controller.ModelState.AddModelError("Test", "This is a test model error");

            var model = new SetLocationViewModel() { DefaultLocationLatitude = 2F, DefaultLocationLongitude = 2F };

            // Act
            var result = await controller.SetLocationAsync(model);

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
            //Assert.IsType<String>(objectResult.Value);
            
            var expected = new SerializableError
                {
                    { "Test", new[] {"This is a test model error"}},
                };

            objectResult.Value.Should().BeOfType<SerializableError>();
            objectResult.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SetLocationAsync_ReturnsNotFound_WithUserManagerReturnsNull()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .Returns(Task.FromResult<ApplicationUser>(null));

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new SetLocationViewModel() { DefaultLocationLatitude = 2F, DefaultLocationLongitude = 2F };

            // Act
            var result = await controller.SetLocationAsync(model);

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("User not found", objectResult.Value);
        }

        [Fact]
        public async Task SetLocationAsync_ReturnsBadRequestWithStringObject_WhenExceptionIsRaised()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ThrowsAsync(new InvalidOperationException());

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new SetLocationViewModel() { DefaultLocationLatitude = 2F, DefaultLocationLongitude = 2F };

            // Act
            var result = await controller.SetLocationAsync(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("There was an error updating the user", objectResult.Value);
        }

        [Fact]
        public async Task SetLocationAsync_ReturnsOK_WhenIdentityResultIsNotSuccessful()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(GetValidTestUser());
            mockUserManager.Setup(repo => repo.UpdateAsync(It.IsAny<ApplicationUser>()))
                           .Returns(Task.FromResult(IdentityResult.Failed()));

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new SetLocationViewModel() { DefaultLocationLatitude = 2F, DefaultLocationLongitude = 2F };

            // Act
            var result = await controller.SetLocationAsync(model);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            var returnObject = Assert.IsType<string>(objectResult.Value);
            Assert.Equal("There was an error updating the user", returnObject);
        }

        [Fact]
        public async Task SetLocationAsync_ReturnsOK_WhenSuccessful()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(GetValidTestUser());
            mockUserManager.Setup(repo => repo.UpdateAsync(It.IsAny<ApplicationUser>()))
                           .Returns(Task.FromResult(IdentityResult.Success));

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new SetLocationViewModel() { DefaultLocationLatitude = 2F, DefaultLocationLongitude = 2F };

            // Act
            var result = await controller.SetLocationAsync(model);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var returnObject = Assert.IsType<SetLocationViewModel>(objectResult.Value);
            Assert.Equal(model, returnObject);
        }

        #endregion


        #region ChangePasswordAsync unit tests

        [Fact]
        public async Task ChangePasswordAsync_ReturnsBadRequest_WithModelStateError()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            //Add model error
            controller.ModelState.AddModelError("Test", "This is a test model error");

            var model = new ChangePasswordViewModel() { };

            // Act
            var result = await controller.ChangePasswordAsync(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
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
        public async Task ChangePasswordAsync_ReturnsNotFound_WithUserManagerReturnsNull()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .Returns(Task.FromResult<ApplicationUser>(null));

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new ChangePasswordViewModel() { };

            // Act
            var result = await controller.ChangePasswordAsync(model);

            // Assert
            var objectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<String>(objectResult.Value);
            Assert.Equal("User not found", objectResult.Value);
        }

        [Fact]
        public async Task ChangePasswordAsync_ReturnsBadRequestWithStringObject_WhenExceptionIsRaised()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ThrowsAsync(new InvalidOperationException());

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new ChangePasswordViewModel() { };

            // Act
            var result = await controller.ChangePasswordAsync(model);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal("There was an error updating the user", objectResult.Value);
        }


        [Fact]
        public async Task ChangePasswordAsync_ReturnsOK_WhenIdentityResultIsNotSuccessful()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(GetValidTestUser());
            mockUserManager.Setup(repo => repo.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                           .Returns(Task.FromResult(IdentityResult.Failed()));

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new ChangePasswordViewModel() { };

            // Act
            var result = await controller.ChangePasswordAsync(model);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is BadRequestObjectResult);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            var returnObject = Assert.IsType<string>(objectResult.Value);
            Assert.Equal("There was an error updating the user", returnObject);
        }

        [Fact]
        public async Task ChangePasswordAsync_ReturnsOK_WhenSuccessful()
        {
            // Arrange
            var mockUserManager = SharedFunctions.InitialiseMockUserManager();
            mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync(GetValidTestUser());
            mockUserManager.Setup(repo => repo.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                           .Returns(Task.FromResult(IdentityResult.Success));

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object, _fileClient.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal("example name") }
            };

            var model = new ChangePasswordViewModel() { };

            // Act
            var result = await controller.ChangePasswordAsync(model);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(objectResult);
            Assert.True(objectResult is OkObjectResult);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            var returnObject = Assert.IsType<ChangePasswordViewModel>(objectResult.Value);
            Assert.Equal(model, returnObject);
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