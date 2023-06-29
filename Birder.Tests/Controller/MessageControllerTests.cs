using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;

namespace Birder.Tests.Controller;

public class MessageControllerTests
{
    [Fact]
    public async Task Returns_200_When_Ok()
    {
        // Arrange
        IOptions<ConfigOptions> testOptions = Options.Create<ConfigOptions>(new ConfigOptions()
        { BaseUrl = "http://localhost:55722", TokenKey = "fgjiorgjivjbrihgnvrHeij45lk45lmf", DevMail = "a@b.com" });

        Mock<ILogger<MessageController>> loggerMock = new();
        var mockService = new Mock<IEmailSender>();
        mockService.Setup(a => a.CreateMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
            .Returns(new SendGridMessage());

        var controller = new MessageController(mockService.Object, loggerMock.Object, testOptions);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        };

        // Act
        var result = await controller.PostContactMessageAsync(new ContactFormDto());

        // Assert
        var objectResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(StatusCodes.Status204NoContent, objectResult.StatusCode);
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