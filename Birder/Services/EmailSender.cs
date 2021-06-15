using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Birder.Services
{
    public interface IEmailSender
    {
        Task SendTemplateEmail(string templateId, string recipient, object model);
    }

    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; }

        public Task SendTemplateEmail(string templateId, string recipient, object model)
        {
            if (string.IsNullOrEmpty(templateId)) 
                throw new ArgumentException($"The argument is null or empty", nameof(templateId));

            if (string.IsNullOrEmpty(recipient))
                throw new ArgumentException($"The argument is null or empty", nameof(recipient));

            if (model is null)
                throw new ArgumentException($"The argument is null", nameof(model));


            //var client = new SendGridClient(Options.SendGridKey);
            //This initialisation means an exception is thrown (otherwise it is a silent failure).
            //see https://github.com/sendgrid/sendgrid-csharp/blob/main/TROUBLESHOOTING.md#error
            var client = new SendGridClient(new SendGridClientOptions { ApiKey = Options.SendGridKey, HttpErrorAsException = true });

            var message = new SendGridMessage();

            message.SetTemplateId(templateId);
            message.SetTemplateData(model);
            message.SetFrom("noreply@birderweb.com", "Birder Administrator");
            message.AddTo(new EmailAddress(recipient));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            message.SetClickTracking(false, false);

            return client.SendEmailAsync(message);
        }
    }
}
