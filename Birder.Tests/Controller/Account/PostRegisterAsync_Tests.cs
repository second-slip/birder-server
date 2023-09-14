using SendGrid.Helpers.Mail;

namespace Birder.Tests.Controller;

public class PostRegisterAsync_Tests
{
    [Fact]
    public async Task PostRegisterAsync_Returns_500_On_Exception()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        var EMPTY_TEST_MODEL = new RegisterViewModel() { };

        mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .ThrowsAsync(new InvalidOperationException());

        // Act
        var result = await controller.PostRegisterAsync(EMPTY_TEST_MODEL);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        mockUserManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        mockUserManager.Verify(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()), Times.Never);
        urlService.Verify(x => x.GetConfirmEmailUrl(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        emailSender.Verify(x => x.CreateMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        emailSender.Verify(x => x.SendMessageAsync(It.IsAny<SendGridMessage>()), Times.Never);
    }

    [Fact]
    public async Task PostRegisterAsync_Returns_500_On_Failure()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        var EMPTY_TEST_MODEL = new RegisterViewModel() { };

        mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Failed(new IdentityError() { Code = string.Empty, Description = "Test IdentityError" })));

        // Act
        var result = await controller.PostRegisterAsync(EMPTY_TEST_MODEL);

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        mockUserManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        mockUserManager.Verify(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()), Times.Never);
        urlService.Verify(x => x.GetConfirmEmailUrl(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        emailSender.Verify(x => x.CreateMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        emailSender.Verify(x => x.SendMessageAsync(It.IsAny<SendGridMessage>()), Times.Never);
    }

    [Fact]
    public async Task PostRegisterAsync_Returns_200_On_Success()
    {
        // Arrange
        var mockUserManager = SharedFunctions.InitialiseMockUserManager();
        var systemClock = new Mock<ISystemClockService>();
        var logger = new Mock<ILogger<AccountController>>();
        var urlService = new Mock<IUrlService>();
        var emailSender = new Mock<IEmailSender>();
        var controller = new AccountController(systemClock.Object, urlService.Object, emailSender.Object, logger.Object, mockUserManager.Object);

        var EMPTY_TEST_MODEL = new RegisterViewModel() { };

        mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(IdentityResult.Success));

        // Act
        var result = await controller.PostRegisterAsync(EMPTY_TEST_MODEL);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { success = true };
        objectResult.Value.Should().BeEquivalentTo(expected);

        mockUserManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        mockUserManager.Verify(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>()), Times.Once);
        urlService.Verify(x => x.GetConfirmEmailUrl(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        emailSender.Verify(x => x.CreateMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);
        emailSender.Verify(x => x.SendMessageAsync(It.IsAny<SendGridMessage>()), Times.Once);
    }
}