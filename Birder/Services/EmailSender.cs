using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Birder.Services;

public interface IEmailSender
{
    Task<bool> SendMessageAsync(SendGridMessage mailMessage);
    SendGridMessage CreateMailMessage(string templateId, string recipient, object model);
}

public class EmailSender : IEmailSender
{
    private ConfigOptions _options { get; }
    private ISendGridClient _sendGridClient;

    public EmailSender(IOptions<ConfigOptions> optionsAccessor,
    ISendGridClient sendGridClient)
    {
        _options = optionsAccessor.Value;
        _sendGridClient = sendGridClient;
    }

    public async Task<bool> SendMessageAsync(SendGridMessage mailMessage)
    {
        var response = await _sendGridClient.SendEmailAsync(mailMessage).ConfigureAwait(false); // await client.SendEmailAsync(mailMessage);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        // try again...
        var secondResponse = await _sendGridClient.SendEmailAsync(mailMessage).ConfigureAwait(false);
        if (secondResponse.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }

    public SendGridMessage CreateMailMessage(string templateId, string recipient, object model)
    {
        if (string.IsNullOrEmpty(templateId))
            throw new ArgumentException($"The argument is null or empty", nameof(templateId));

        if (string.IsNullOrEmpty(recipient))
            throw new ArgumentException($"The argument is null or empty", nameof(recipient));

        if (model is null)
            throw new ArgumentException($"The argument is null", nameof(model));

        var message = MailHelper.CreateSingleTemplateEmail(new EmailAddress(_options.SendGridMail), new EmailAddress(recipient), templateId, model);
        message.SetClickTracking(true, true); // Disable click tracking: see https://sendgrid.com/docs/User_Guide/Settings/tracking.html

        return message;
    }
}