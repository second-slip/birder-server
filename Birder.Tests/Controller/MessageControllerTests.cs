using Birder.Controllers;
using Birder.Services;
using Birder.TestsHelpers;
using Birder.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Controller
{
    public class MessageControllerTests
    {
        private readonly Mock<ILogger<MessageController>> _logger;
        private readonly Mock<IEmailSender> _emailSender;

        public MessageControllerTests()
        {
            _logger = new Mock<ILogger<MessageController>>();
            _emailSender = new Mock<IEmailSender>();
        }

        [Fact]
        public async Task Returns_200_When_Ok()
        {
            // Arrange
            _emailSender.Setup(a => a.SendTemplateEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.CompletedTask);

            var controller = new MessageController(_emailSender.Object, _logger.Object);

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
            _emailSender.Setup(a => a.SendTemplateEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .ThrowsAsync(new InvalidOperationException());

            var controller = new MessageController(_emailSender.Object, _logger.Object);

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

        //[Fact]
        //public async Task Returns_400_Response_When_ModelState_Is_Invalid()
        //{
        //    // Arrange
        //    var controller = new MessageController(_emailSender.Object, _logger.Object);

        //    controller.ControllerContext = new ControllerContext()
        //    {
        //        HttpContext = new DefaultHttpContext()
        //        { User = SharedFunctions.GetTestClaimsPrincipal("test_username") }
        //    };

        //    controller.ModelState.AddModelError("Test", "This is a test model error");

        //    // Act
        //    var result = await controller.PostContactMessageAsync(new ContactFormDto());

        //    // Assert
        //    var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        //    Assert.NotNull(objectResult);
        //    Assert.True(objectResult is BadRequestObjectResult);
        //    Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);

        //    var expected = new SerializableError
        //        {
        //            { "Test", new[] {"This is a test model error"}},
        //        };

        //    objectResult.Value.Should().BeOfType<SerializableError>();
        //    objectResult.Value.Should().BeEquivalentTo(expected);
        //}
    }
}
