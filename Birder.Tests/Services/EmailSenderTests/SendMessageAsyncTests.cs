using System.Net;
using System.Threading;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Birder.Tests.Services;

public class SendMessageAsyncTests
{
    [Fact]
    public async Task SendEmailAsync_Returns_True_When_Success_On_First_Try()
    {
        var someOptions = Options.Create(new ConfigOptions());
        var mock = new Mock<ISendGridClient>();

        mock.Setup(x => x.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Response(HttpStatusCode.OK, null, null)));
        // .Returns(Task.FromResult((SendGrid.Response)null)); //<-- needed to allow async flow to continue
        // .ReturnsAsync(Task.FromResult(new Response(HttpStatusCode.OK, null, null)));

        var service = new EmailSender(someOptions, mock.Object);

        // Act
        var result = await service.SendMessageAsync(It.IsAny<SendGridMessage>());

        // Assert
        Assert.IsType<bool>(result);
        result.ShouldEqual(true);
        mock.Verify(x => x.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SendEmailAsync_Returns_True_When_Success_On_Second_Try()
    {
        var someOptions = Options.Create(new ConfigOptions());
        var mock = new Mock<ISendGridClient>();

        mock.SetupSequence(x => x.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Response(HttpStatusCode.BadRequest, null, null)))
                .Returns(Task.FromResult(new Response(HttpStatusCode.OK, null, null)));

        var service = new EmailSender(someOptions, mock.Object);

        // Act
        var result = await service.SendMessageAsync(It.IsAny<SendGridMessage>());

        // Assert
        Assert.IsType<bool>(result);
        result.ShouldEqual(true);

        mock.Verify(x => x.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task SendEmailAsync_Returns_True_When_Fail_On_Both_Tries()
    {
        var someOptions = Options.Create(new ConfigOptions());
        var mock = new Mock<ISendGridClient>();

        mock.SetupSequence(x => x.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new Response(HttpStatusCode.BadRequest, null, null)))
                .Returns(Task.FromResult(new Response(HttpStatusCode.BadRequest, null, null)));

        var service = new EmailSender(someOptions, mock.Object);

        // Act
        var result = await service.SendMessageAsync(It.IsAny<SendGridMessage>());

        // Assert
        Assert.IsType<bool>(result);
        result.ShouldEqual(false);

        mock.Verify(x => x.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }
}