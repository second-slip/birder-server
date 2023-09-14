namespace Birder.Tests.Controller;

public class PostResetPasswordAsync_Tests
{
    [Fact]
    public async Task PostResetPasswordAsync_Returns_500_When_UserManager_Returns_Null()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        mockUserManager.Setup(repo => repo.FindByNameAsync(It.IsAny<string>()))
                       .Returns(Task.FromResult<ApplicationUser>(null));
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        var EMPTY_TEST_MODEL = new ResetPasswordViewModel() { };

        // Act
        var result = await controller.PostResetPasswordAsync(EMPTY_TEST_MODEL);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { success = true };
        objectResult.Value.Should().BeEquivalentTo(expected);

        mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        mockUserManager.Verify(x => x.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task PostResetPasswordAsync_Returns_500_On_Exception()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .ThrowsAsync(new InvalidOperationException());

        var EMPTY_TEST_MODEL = new ResetPasswordViewModel() { };

        // Act
        var result = await controller.PostResetPasswordAsync(EMPTY_TEST_MODEL);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        mockUserManager.Verify(x => x.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task PostResetPasswordAsync_Returns_500_On_Failure()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        var user = new ApplicationUser()
        {
            Email = "a@b.com",
            EmailConfirmed = true,
            UserName = "Test",
            Avatar = "",
            DefaultLocationLongitude = 3F,
            DefaultLocationLatitude = 3F
        };
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(user);
        mockUserManager.Setup(repo => repo.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Failed()));

        var EMPTY_TEST_MODEL = new ResetPasswordViewModel() { };

        // Act
        var result = await controller.PostResetPasswordAsync(EMPTY_TEST_MODEL);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        mockUserManager.Verify(x => x.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task PostResetPasswordAsync_Returns_500_On_Success()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        var user = new ApplicationUser()
        {
            Email = "a@b.com",
            EmailConfirmed = true,
            UserName = "Test",
            Avatar = "",
            DefaultLocationLongitude = 3F,
            DefaultLocationLatitude = 3F
        };
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(user);
        mockUserManager.Setup(repo => repo.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Success));

        var EMPTY_TEST_MODEL = new ResetPasswordViewModel() { };

        // Act
        var result = await controller.PostResetPasswordAsync(EMPTY_TEST_MODEL);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { success = true };
        objectResult.Value.Should().BeEquivalentTo(expected);

        mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        mockUserManager.Verify(x => x.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }


}

