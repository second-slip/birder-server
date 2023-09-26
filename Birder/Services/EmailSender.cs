using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Birder.Services;

public interface IEmailSender
{
    Task SendMessageAsync(SendGridMessage mailMessage);
    SendGridMessage CreateMailMessage(string templateId, string recipient, object model);
}

public class EmailSender : IEmailSender
{
    public AuthMessageSenderOptions Options { get; }

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
    {
        Options = optionsAccessor.Value;
    }

    public async Task SendMessageAsync(SendGridMessage mailMessage)
    {
        var options = new SendGridClientOptions { ApiKey = Options.SendGridKey, HttpErrorAsException = true };
        var client = new SendGridClient(options);
        await client.SendEmailAsync(mailMessage);
    }

    public SendGridMessage CreateMailMessage(string templateId, string recipient, object model)
    {
        if (string.IsNullOrEmpty(templateId))
            throw new ArgumentException($"The argument is null or empty", nameof(templateId));

        if (string.IsNullOrEmpty(recipient))
            throw new ArgumentException($"The argument is null or empty", nameof(recipient));

        if (model is null)
            throw new ArgumentException($"The argument is null", nameof(model));

        var message = new SendGridMessage();
        message.SetTemplateId(templateId);
        message.SetTemplateData(model);
        message.SetFrom("andrew-stuart-cross@outlook.com", "Birder Administrator");
        message.AddTo(new EmailAddress(recipient));
        message.SetClickTracking(false, false); // Disable click tracking: see https://sendgrid.com/docs/User_Guide/Settings/tracking.html

        return message;
    }
}