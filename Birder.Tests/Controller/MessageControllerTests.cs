using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;

namespace Birder.Tests.Controller;

public class MessageControllerTests
{
    [Fact]
    public async Task Returns_200_When_Ok_With_Success_True_When_Message_Is_Sent()
    {
        // Arrange
        IOptions<ConfigOptions> testOptions = Options.Create<ConfigOptions>(new ConfigOptions()
        { BaseUrl = "http://localhost:55722", TokenKey = "fgjiorgjivjbrihgnvrHeij45lk45lmf", DevMail = "a@b.com" });


        Mock<ILogger<MessageController>> loggerMock = new();
        var mockService = new Mock<IEmailSender>();
        mockService.Setup(a => a.CreateMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
            .Returns(new SendGridMessage());
        mockService.Setup(a => a.SendMessageAsync(It.IsAny<SendGridMessage>()))
            .ReturnsAsync(true);

        var controller = new MessageController(mockService.Object, loggerMock.Object, testOptions);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        };

        // Act
        var result = await controller.PostContactMessageAsync(new ContactFormDto());

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { success = true };
        objectResult.Value.Should().BeEquivalentTo(expected);
    }

        [Fact]
    public async Task Returns_200_When_Ok_With_Success_False_When_Message_Is_Not_Sent()
    {
        // Arrange
        IOptions<ConfigOptions> testOptions = Options.Create<ConfigOptions>(new ConfigOptions()
        { BaseUrl = "http://localhost:55722", TokenKey = "fgjiorgjivjbrihgnvrHeij45lk45lmf", DevMail = "a@b.com" });


        Mock<ILogger<MessageController>> loggerMock = new();
        var mockService = new Mock<IEmailSender>();
        mockService.Setup(a => a.CreateMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
            .Returns(new SendGridMessage());
        mockService.Setup(a => a.SendMessageAsync(It.IsAny<SendGridMessage>()))
            .ReturnsAsync(false);

        var controller = new MessageController(mockService.Object, loggerMock.Object, testOptions);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        };

        // Act
        var result = await controller.PostContactMessageAsync(new ContactFormDto());

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var expected = new { success = false };
        objectResult.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Returns_500_When_Exception_Is_Raised()
    {
        // Arrange
        IOptions<ConfigOptions> testOptions = Options.Create<ConfigOptions>(new ConfigOptions()
        { BaseUrl = "http://localhost:55722", TokenKey = "fgjiorgjivjbrihgnvrHeij45lk45lmf", DevMail = "a@b.com" });

        Mock<ILogger<MessageController>> loggerMock = new();
        var mockService = new Mock<IEmailSender>();
        mockService.Setup(a => a.CreateMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
            .Throws(new InvalidOperationException());

        var controller = new MessageController(mockService.Object, loggerMock.Object, testOptions);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        };

        // Act
        var result = await controller.PostContactMessageAsync(new ContactFormDto());

        // Assert
        var objectResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }
}