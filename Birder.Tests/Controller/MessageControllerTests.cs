using SendGrid.Helpers.Mail;

namespace Birder.Tests.Controller;

public class MessageControllerTests
{
    [Fact]
    public async Task Returns_200_When_Ok()
    {
        // Arrange
        Mock<ILogger<MessageController>> loggerMock = new();
        var mockService = new Mock<IEmailSender>();
        mockService.Setup(a => a.CreateMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
            .Returns(new SendGridMessage());
        //.Returns(Task.CompletedTask);

        var controller = new MessageController(mockService.Object, loggerMock.Object);

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
        var actual = Assert.IsType<ContactFormDto>(objectResult.Value);
    }

    [Fact]
    public async Task Returns_500_When_Exception_Is_Raised()
    {
        // Arrange
        Mock<ILogger<MessageController>> loggerMock = new();
        var mockService = new Mock<IEmailSender>();
        mockService.Setup(a => a.CreateMailMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
            .Throws(new InvalidOperationException());

        var controller = new MessageController(mockService.Object, loggerMock.Object);

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        };

        // Act
        var result = await controller.PostContactMessageAsync(new ContactFormDto());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        var actual = Assert.IsType<string>(objectResult.Value);
        Assert.Equal($"an unexpected error occurred", actual);
    }
}