// namespace Birder.Tests.Controller;

// public class GetConfirmEmailAsync_Tests
// {



//     [Fact]
//     public async Task ConfirmEmailAsync_ReturnsBadRequest_WhenUsernameArgumentIsNull()
//     {
//         // Arrange
//         var mockUserManager = SharedFunctions.InitialiseMockUserManager();

//         string testUsername = null;
//         string testCode = string.Empty;

//         var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

//         // Act
//         var result = await controller.GetConfirmEmailAsync(testUsername, testCode);

//         // Assert
//         var objectResult = Assert.IsType<StatusCodeResult>(result);
//         // Assert.NotNull(objectResult);
//         // Assert.True(objectResult is BadRequestObjectResult);
//         Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

//         // var expected = "An error occurred";
//         // objectResult.Value.Should().BeOfType<string>();
//         // objectResult.Value.Should().BeEquivalentTo(expected);
//     }

//     [Fact]
//     public async Task ConfirmEmailAsync_ReturnsBadRequest_WhenCodeArgumentIsNull()
//     {
//         // Arrange
//         var mockUserManager = SharedFunctions.InitialiseMockUserManager();

//         string testUsername = string.Empty;
//         string testCode = null;

//         var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

//         // Act
//         var result = await controller.GetConfirmEmailAsync(testUsername, testCode);

//         // Assert
//         var objectResult = Assert.IsType<StatusCodeResult>(result);
//         // Assert.NotNull(objectResult);
//         // Assert.True(objectResult is BadRequestObjectResult);
//         Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

//         // var expected = "An error occurred";
//         // objectResult.Value.Should().BeOfType<string>();
//         // objectResult.Value.Should().BeEquivalentTo(expected);
//     }

//     [Fact]
//     public async Task ConfirmEmailAsync_ReturnsNotFound_WhenRepositoryReturnsNull()
//     {
//         // Arrange
//         var mockUserManager = SharedFunctions.InitialiseMockUserManager();
//         mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
//                        .Returns(Task.FromResult<ApplicationUser>(null));

//         string testUsername = string.Empty;
//         string testCode = string.Empty;

//         var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

//         // Act
//         var result = await controller.GetConfirmEmailAsync(testUsername, testCode);

//         // Assert
//         var objectResult = Assert.IsType<StatusCodeResult>(result);
//         Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
//         // var expected = "An error occurred";
//         // objectResult.Value.Should().BeOfType<string>();
//         // objectResult.Value.Should().BeEquivalentTo(expected);
//     }

//     [Fact]
//     public async Task ConfirmEmailAsync_ReturnsBadRequest_WhenExceptionIsRaised()
//     {
//         // Arrange
//         var mockUserManager = SharedFunctions.InitialiseMockUserManager();
//         mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
//                        .ThrowsAsync(new InvalidOperationException());

//         string testUsername = string.Empty;
//         string testCode = string.Empty;

//         var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

//         // Act
//         var result = await controller.GetConfirmEmailAsync(testUsername, testCode);

//         // Assert
//         var objectResult = Assert.IsType<StatusCodeResult>(result);
//         // Assert.NotNull(objectResult);
//         // Assert.True(objectResult is BadRequestObjectResult);
//         Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

//         // var expected = "An error occurred";
//         // objectResult.Value.Should().BeOfType<string>();
//         // objectResult.Value.Should().BeEquivalentTo(expected);
//     }

//     [Fact]
//     public async Task ConfirmEmailAsync_BadRequest_WhenConfirmEmailAsyncFails()
//     {
//         // Arrange
//         var mockUserManager = SharedFunctions.InitialiseMockUserManager();
//         mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
//                        .ReturnsAsync(GetValidTestUser(true));
//         mockUserManager.Setup(repo => repo.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
//                        .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = string.Empty, Description = "Test IdentityError" })));

//         string testUsername = string.Empty;
//         string testCode = string.Empty;

//         var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

//         // Act
//         var result = await controller.GetConfirmEmailAsync(testUsername, testCode);

//         // Assert
//         var objectResult = Assert.IsType<StatusCodeResult>(result);
//         Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

//         // var expected = "An error occurred";
//         // objectResult.Value.Should().BeOfType<string>();
//         // objectResult.Value.Should().BeEquivalentTo(expected);
//     }

//     [Fact]
//     public async Task ConfirmEmailAsync_BadRequest_WhenConfirmEmailAsyncIsSuccessful()
//     {
//         // Arrange
//         var mockUserManager = SharedFunctions.InitialiseMockUserManager();
//         mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
//                        .ReturnsAsync(GetValidTestUser(true));
//         mockUserManager.Setup(repo => repo.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
//                        .Returns(Task.FromResult(IdentityResult.Success));

//         string testUsername = "test_string";
//         string testCode = string.Empty;

//         var controller = new AccountController(_systemClock.Object, _urlService.Object, _emailSender.Object, _logger.Object, mockUserManager.Object);

//         // Act
//         var result = await controller.GetConfirmEmailAsync(testUsername, testCode);

//         // Assert
//         var objectResult = Assert.IsType<StatusCodeResult>(result);
//         // Assert.NotNull(objectResult);
//         // Assert.True(objectResult is RedirectResult);

//         // var expected = "/confirmed-email";
//         // objectResult.Url.Should().BeOfType<string>();
//         // objectResult.Url.Should().BeEquivalentTo(expected);
//     }



// }