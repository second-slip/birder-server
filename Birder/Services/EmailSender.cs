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
    public AuthMessageSenderOptions Options { get; }

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
    {
        Options = optionsAccessor.Value;
    }

    public async Task<bool> SendMessageAsync(SendGridMessage mailMessage)
    {
        var options = new SendGridClientOptions { ApiKey = Options.SendGridKey, HttpErrorAsException = true };
        var client = new SendGridClient(options);

        var response = await client.SendEmailAsync(mailMessage).ConfigureAwait(false); // await client.SendEmailAsync(mailMessage);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        // try again...
        var secondResponse = await client.SendEmailAsync(mailMessage).ConfigureAwait(false);
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

        var message = MailHelper.CreateSingleTemplateEmail(new EmailAddress(Options.SendGridMail), new EmailAddress(recipient),templateId, model);// .CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        // var message = new SendGridMessage();
        // message.SetTemplateId(templateId);
        // message.SetTemplateData(model);
        // message.SetFrom("andrew-stuart-cross@outlook.com", "Birder Administrator");
        // message.AddTo(new EmailAddress(recipient));
        message.SetClickTracking(true, true); // Disable click tracking: see https://sendgrid.com/docs/User_Guide/Settings/tracking.html

        return message;
    }
}