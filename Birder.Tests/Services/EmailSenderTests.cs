using Microsoft.Extensions.Options;

namespace Birder.Tests.Services;

public class EmailSenderTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Returns_ArgumentException_If_templateId_Argument_Is_Null_Or_Empty(string templateId)
    {
        // Arrange
        var someOptions = Options.Create(new AuthMessageSenderOptions());
        var service = new EmailSender(someOptions);

        // Act & Assert
        var result = await Assert.ThrowsAsync<ArgumentException>(() => service.SendTemplateEmail(templateId, "recipient", It.IsAny<object>()));
        Assert.Equal($"The argument is null or empty (Parameter 'templateId')", result.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Returns_ArgumentException_If_recipient_Argument_Is_Null_Or_Empty(string recipient)
    {
        // Arrange
        var someOptions = Options.Create(new AuthMessageSenderOptions());
        var service = new EmailSender(someOptions);

        //Act & Assert
        var result = await Assert.ThrowsAsync<ArgumentException>(() => service.SendTemplateEmail("templateId", recipient, It.IsAny<object>()));
        Assert.Equal($"The argument is null or empty (Parameter 'recipient')", result.Message);
    }

    [Fact]
    public async Task Returns_ArgumentException_If_model_Argument_Is_Null()
    {
        // Arrange
        var someOptions = Options.Create(new AuthMessageSenderOptions());
        var service = new EmailSender(someOptions);

        //Act & Assert
        var result = await Assert.ThrowsAsync<ArgumentException>(() => service.SendTemplateEmail("templateId", "recipient", null));
        Assert.Equal($"The argument is null (Parameter 'model')", result.Message);
    }
}