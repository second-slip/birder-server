using AutoMapper;
using Birder.Controllers;
using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Birder.Services;
using Birder.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        public ManageControllerTests()
        {
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

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
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

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
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

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
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

            var controller = new ManageController(_mapper, _emailSender.Object, _urlService.Object, _logger.Object, mockUserManager.Object);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
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
        }


        #endregion




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
    }
}
