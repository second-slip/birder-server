// namespace Birder.Tests.Controller;

// public class MessageControllerTests
// {
//     private readonly Mock<ILogger<MessageController>> _logger;
//     private readonly Mock<IEmailSender> _emailSender;

//     public MessageControllerTests()
//     {
//         _logger = new Mock<ILogger<MessageController>>();
//         _emailSender = new Mock<IEmailSender>();
//     }

//     [Fact]
//     public async Task Returns_200_When_Ok()
//     {
//         // Arrange
//         _emailSender.Setup(a => a.SendTemplateEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
//             .Returns(Task.CompletedTask);

//         var controller = new MessageController(_emailSender.Object, _logger.Object);

//         controller.ControllerContext = new ControllerContext()
//         {
//             HttpContext = new DefaultHttpContext()
//             { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
//         };

//         // Act
//         var result = await controller.PostContactMessageAsync(new ContactFormDto());

//         // Assert
//         var objectResult = Assert.IsType<OkObjectResult>(result);
//         Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
//         var actual = Assert.IsType<ContactFormDto>(objectResult.Value);
//     }

//     [Fact]
//     public async Task Returns_500_When_Exception_Is_Raised()
//     {
//         // Arrange
//         _emailSender.Setup(a => a.SendTemplateEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
//             .ThrowsAsync(new InvalidOperationException());

//         var controller = new MessageController(_emailSender.Object, _logger.Object);

//         controller.ControllerContext = new ControllerContext()
//         {
//             HttpContext = new DefaultHttpContext()
//             { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
//         };

//         // Act
//         var result = await controller.PostContactMessageAsync(new ContactFormDto());

//         // Assert
//         var objectResult = Assert.IsType<ObjectResult>(result);
//         Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
//         var actual = Assert.IsType<string>(objectResult.Value);
//         Assert.Equal($"an unexpected error occurred", actual);
//     }
// }