using System;
using System.Collections.Generic;
using System.Text;

namespace Birder.Tests.Controller
{
    public class NetworkControllerTests
    {



        //#region Follow action tests

        //[Fact]
        //public async Task PostFollowUserAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IUserRepository>();

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        //    };

        //    //Add model error
        //    controller.ModelState.AddModelError("Test", "This is a test model error");

        //    // Act
        //    var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel());

        //    var modelState = controller.ModelState;
        //    Assert.Equal(1, modelState.ErrorCount);
        //    Assert.True(modelState.ContainsKey("Test"));
        //    Assert.True(modelState["Test"].Errors.Count == 1);
        //    Assert.Equal("This is a test model error", modelState["Test"].Errors[0].ErrorMessage);

        //    // test response
        //    var objectResult = result as ObjectResult;
        //    Assert.NotNull(objectResult);
        //    Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.True(objectResult is BadRequestObjectResult);
        //    Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        //    //
        //    Assert.IsType<String>(objectResult.Value);

        //    //Assert.Contains("This is a test model error", "This is a test model error");
        //    Assert.Equal("Invalid modelstate", objectResult.Value);
        //}

        //[Fact]
        //public async Task PostFollowUserAsync_ReturnsNotFound_WhenUserIsNullFromRepository()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IUserRepository>();
        //    mockRepo.Setup(x => x.GetUserAndNetworkAsync(It.IsAny<String>()))
        //            .Returns(Task.FromResult<ApplicationUser>(null));

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        //    };

        //    // Act
        //    var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel());

        //    // Assert
        //    var objectResult = result as ObjectResult;
        //    Assert.NotNull(objectResult);
        //    Assert.IsType<NotFoundObjectResult>(result);
        //    Assert.True(objectResult is NotFoundObjectResult);
        //    Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        //    Assert.IsType<String>(objectResult.Value);
        //    Assert.Equal("User not found", objectResult.Value);
        //}

        //[Fact]
        //public async Task PostFollowUserAsync_ReturnsBadRequest_FollowerAndToFollowAreEqual()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IUserRepository>();
        //    mockRepo.Setup(x => x.GetUserAndNetworkAsync(It.IsAny<String>()))
        //            .ReturnsAsync(GetOwnUserProfile());

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        //    };

        //    // Act
        //    var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel());

        //    // Assert
        //    var objectResult = result as ObjectResult;
        //    Assert.NotNull(objectResult);
        //    Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.True(objectResult is BadRequestObjectResult);
        //    Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        //    Assert.IsType<String>(objectResult.Value);
        //    Assert.Equal("Trying to follow yourself", objectResult.Value);
        //}

        //[Fact]
        //public async Task PostFollowUserAsync_ReturnsBadRequestWithStringObject_WhenExceptionIsRaised()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IUserRepository>();
        //    mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
        //         .ThrowsAsync(new InvalidOperationException());

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        //    };

        //    // Act
        //    var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel());

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result);
        //    var objectResult = result as ObjectResult;
        //    Assert.Equal("An error occurred trying to follow user: Test Network View Model", objectResult.Value);
        //}

        //[Fact]
        //public async Task PostFollowUserAsync_ReturnsOkObject_WhenRequestIsValid()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IUserRepository>();
        //    mockRepo.SetupSequence(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
        //        .ReturnsAsync(GetOwnUserProfile())
        //        .ReturnsAsync(GetOtherMemberUserProfile())
        //        .ReturnsAsync(GetOwnUserProfile());

        //    mockRepo.Setup(repo => repo.Follow(It.IsAny<ApplicationUser>(), It.IsAny<ApplicationUser>()))
        //        .Verifiable();

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();
        //    mockUnitOfWork.Setup(x => x.CompleteAsync()).Returns(Task.CompletedTask);

        //    var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        //    };

        //    // Act
        //    var result = await controller.PostFollowUserAsync(GetTestNetworkUserViewModel());

        //    // Assert
        //    var objectResult = result as ObjectResult;
        //    Assert.NotNull(objectResult);
        //    Assert.IsType<OkObjectResult>(result);
        //    Assert.True(objectResult is OkObjectResult);
        //    Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        //    Assert.IsType<NetworkUserViewModel>(objectResult.Value);

        //    var model = objectResult.Value as NetworkUserViewModel;
        //    Assert.Equal("Other Member's Profile Test", model.UserName);
        //}

        //#endregion




        //#region Unfollow action tests

        //[Fact]
        //public async Task PostUnfollowUserAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<INetworkRepository>();

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        //    };

        //    //Add model error
        //    controller.ModelState.AddModelError("Test", "This is a test model error");

        //    // Act
        //    var result = await controller.PostUnfollowUserAsync(GetTestNetworkUserViewModel());

        //    var modelState = controller.ModelState;
        //    Assert.Equal(1, modelState.ErrorCount);
        //    Assert.True(modelState.ContainsKey("Test"));
        //    Assert.True(modelState["Test"].Errors.Count == 1);
        //    Assert.Equal("This is a test model error", modelState["Test"].Errors[0].ErrorMessage);

        //    // test response
        //    var objectResult = result as ObjectResult;
        //    Assert.NotNull(objectResult);
        //    Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.True(objectResult is BadRequestObjectResult);
        //    Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        //    //
        //    Assert.IsType<String>(objectResult.Value);

        //    //Assert.Contains("This is a test model error", "This is a test model error");
        //    Assert.Equal("Invalid modelstate", objectResult.Value);
        //}

        //[Fact]
        //public async Task PostUnfollowUserAsync_ReturnsNotFound_WhenUserIsNullFromRepository()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IUserRepository>();
        //    mockRepo.Setup(x => x.GetUserAndNetworkAsync(It.IsAny<String>()))
        //            .Returns(Task.FromResult<ApplicationUser>(null));

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        //    };

        //    // Act
        //    var result = await controller.PostUnfollowUserAsync(GetTestNetworkUserViewModel());

        //    // Assert
        //    var objectResult = result as ObjectResult;
        //    Assert.NotNull(objectResult);
        //    Assert.IsType<NotFoundObjectResult>(result);
        //    Assert.True(objectResult is NotFoundObjectResult);
        //    Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        //    Assert.IsType<String>(objectResult.Value);
        //    Assert.Equal("User not found", objectResult.Value);
        //}

        //[Fact]
        //public async Task PostUnfollowUserAsync_ReturnsBadRequest_FollowerAndToFollowAreEqual()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IUserRepository>();
        //    mockRepo.Setup(x => x.GetUserAndNetworkAsync(It.IsAny<String>()))
        //            .ReturnsAsync(GetOwnUserProfile());

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        //    };

        //    // Act
        //    var result = await controller.PostUnfollowUserAsync(GetTestNetworkUserViewModel());

        //    // Assert
        //    var objectResult = result as ObjectResult;
        //    Assert.NotNull(objectResult);
        //    Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.True(objectResult is BadRequestObjectResult);
        //    Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        //    Assert.IsType<String>(objectResult.Value);
        //    Assert.Equal("Trying to unfollow yourself", objectResult.Value);
        //}

        //[Fact]
        //public async Task PostUnfollowUserAsync_ReturnsBadRequestWithStringObject_WhenExceptionIsRaised()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IUserRepository>();
        //    mockRepo.Setup(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
        //         .ThrowsAsync(new InvalidOperationException());

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();

        //    var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        //    };

        //    // Act
        //    var result = await controller.PostUnfollowUserAsync(GetTestNetworkUserViewModel());

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result);
        //    var objectResult = result as ObjectResult;
        //    Assert.Equal("An error occurred trying to unfollow user: Test Network View Model", objectResult.Value);
        //}

        //[Fact]
        //public async Task PostUnfollowUserAsync_ReturnsOkObject_WhenRequestIsValid()
        //{
        //    // Arrange
        //    var mockRepo = new Mock<IUserRepository>();
        //    mockRepo.SetupSequence(repo => repo.GetUserAndNetworkAsync(It.IsAny<string>()))
        //        .ReturnsAsync(GetOwnUserProfile())
        //        .ReturnsAsync(GetOtherMemberUserProfile())
        //        .ReturnsAsync(GetOwnUserProfile());

        //    mockRepo.Setup(repo => repo.Follow(It.IsAny<ApplicationUser>(), It.IsAny<ApplicationUser>()))
        //        .Verifiable();

        //    var mockUnitOfWork = new Mock<IUnitOfWork>();
        //    mockUnitOfWork.Setup(x => x.CompleteAsync()).Returns(Task.CompletedTask);

        //    var controller = new UserController(_mapper, mockUnitOfWork.Object, _logger.Object, mockRepo.Object);

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext() { User = SharedFunctions.GetTestClaimsPrincipal() }
        //    };

        //    // Act
        //    var result = await controller.PostUnfollowUserAsync(GetTestNetworkUserViewModel());

        //    // Assert
        //    var objectResult = result as ObjectResult;
        //    Assert.NotNull(objectResult);
        //    Assert.IsType<OkObjectResult>(result);
        //    Assert.True(objectResult is OkObjectResult);
        //    Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        //    Assert.IsType<NetworkUserViewModel>(objectResult.Value);

        //    var model = objectResult.Value as NetworkUserViewModel;
        //    Assert.Equal("Other Member's Profile Test", model.UserName);
        //}

        //#endregion
    }
}
