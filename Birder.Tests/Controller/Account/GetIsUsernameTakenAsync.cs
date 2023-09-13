namespace Birder.Tests.Controller;

public class GetIsUsernameTakenAsync
{
    [Fact]
    public async Task GetIsUsernameAvailableAsync_ReturnsBadRequest_WhenUsernameArgumentIsNull()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        const string USERNAME_ARGUMENT_IS_EMPTY = "";

        // Act
        var result = await controller.GetIsUsernameTakenAsync(USERNAME_ARGUMENT_IS_EMPTY);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
    }

    [Fact]
    public async Task GetIsUsernameAvailableAsync_ReturnsBadRequest_WhenExceptionIsRaised()
    {
        // Arrange
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                       .ThrowsAsync(new InvalidOperationException());

        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        const string USERNAME_ARGUMENT_IS_VALID_STRING = "ANY_USER_NAME";

        // Act
        var result = await controller.GetIsUsernameTakenAsync(USERNAME_ARGUMENT_IS_VALID_STRING);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task GetIsUsernameAvailableAsync_ReturnsBadRequest_WhenUsernameIsTaken()
    {
        // Arrange
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var user = new ApplicationUser()
        {
            Email = "a@b.com",
            EmailConfirmed = true,
            UserName = "Test",
            Avatar = "",
            DefaultLocationLongitude = 3F,
            DefaultLocationLatitude = 3F
        };
        mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                       .ReturnsAsync(user);

        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        const string USERNAME_ARGUMENT_IS_VALID_STRING = "ANY_USER_NAME";

        // Act
        var result = await controller.GetIsUsernameTakenAsync(USERNAME_ARGUMENT_IS_VALID_STRING);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { usernameTaken = true };
        objectResult.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetIsUsernameAvailableAsync_ReturnsOk_WhenUsernameIsAvailable()
    {
        // Arrange
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                       .Returns(Task.FromResult<ApplicationUser>(null));

        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        const string USERNAME_ARGUMENT_IS_VALID_STRING = "ANY_USER_NAME";

        // Act
        var result = await controller.GetIsUsernameTakenAsync(USERNAME_ARGUMENT_IS_VALID_STRING);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { usernameTaken = false };
        objectResult.Value.Should().BeEquivalentTo(expected);
    }
}