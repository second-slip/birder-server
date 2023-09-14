using SendGrid.Helpers.Mail;

namespace Birder.Tests.Controller;

public class PostRequestPasswordResetAsync_Tests
{
    [Fact]
    public async Task PostRequestPasswordResetAsync_Returns_500_When_Repository_Returns_Null()
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

        var EMPTY_TEST_MODEL = new UserEmailDto() { };

        // Act
        var result = await controller.PostRequestPasswordResetAsync(EMPTY_TEST_MODEL);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { success = true };
        objectResult.Value.Should().BeEquivalentTo(expected);

        mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>()), Times.Never);
        urlService.Verify(x => x.GetResetPasswordUrl(It.IsAny<string>()), Times.Never);
        emailSender.Verify(x => x.CreateMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        emailSender.Verify(x => x.SendMessageAsync(It.IsAny<SendGridMessage>()), Times.Never);
    }

    [Fact]
    public async Task PostRequestPasswordResetAsync_Returns_500_On_Exception()
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

        var EMPTY_TEST_MODEL = new UserEmailDto() { };

        // Act
        var result = await controller.PostRequestPasswordResetAsync(EMPTY_TEST_MODEL);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>()), Times.Never);
        urlService.Verify(x => x.GetResetPasswordUrl(It.IsAny<string>()), Times.Never);
        emailSender.Verify(x => x.CreateMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        emailSender.Verify(x => x.SendMessageAsync(It.IsAny<SendGridMessage>()), Times.Never);
    }

    [Fact]
    public async Task PostRequestPasswordResetAsync_Returns_Ok_When_Email_Is_Not_Confirmed()
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
            EmailConfirmed = false, // <---
            UserName = "Test",
            Avatar = "",
            DefaultLocationLongitude = 3F,
            DefaultLocationLatitude = 3F
        };
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(user);

        var EMPTY_TEST_MODEL = new UserEmailDto() { };

        // Act
        var result = await controller.PostRequestPasswordResetAsync(EMPTY_TEST_MODEL);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { success = true };
        objectResult.Value.Should().BeEquivalentTo(expected);

        mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>()), Times.Never);
        urlService.Verify(x => x.GetResetPasswordUrl(It.IsAny<string>()), Times.Never);
        emailSender.Verify(x => x.CreateMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        emailSender.Verify(x => x.SendMessageAsync(It.IsAny<SendGridMessage>()), Times.Never);
    }

    [Fact]
    public async Task PostRequestPasswordResetAsync_ReturnsOk_WhenSuccessful()
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
            EmailConfirmed = true, // <--
            UserName = "Test",
            Avatar = "",
            DefaultLocationLongitude = 3F,
            DefaultLocationLatitude = 3F
        };
        mockUserManager.Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(user);
        var testCode = "myTestCode";
        mockUserManager.Setup(um => um.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>()))
                        .ReturnsAsync(testCode);

        var EMPTY_TEST_MODEL = new UserEmailDto() { };

        // Act
        var result = await controller.PostRequestPasswordResetAsync(EMPTY_TEST_MODEL);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { success = true };
        objectResult.Value.Should().BeEquivalentTo(expected);

        mockUserManager.Verify(x => x.FindByEmailAsync(It.IsAny<string>()), Times.Once);
        mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>()), Times.Once);
        urlService.Verify(x => x.GetResetPasswordUrl(It.IsAny<string>()), Times.Once);
        emailSender.Verify(x => x.CreateMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        emailSender.Verify(x => x.SendMessageAsync(It.IsAny<SendGridMessage>()), Times.Once);
    }
}