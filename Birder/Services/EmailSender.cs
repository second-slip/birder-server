using Birder.Templates;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Birder.Services
{
    public interface IEmailSender
    {
        Task SendEmailConfirmationEmailAsync(ConfirmEmailDto accountDetails);
        Task SendChangedAccountEmailConfirmationEmailAsync(ConfirmEmailDto accountDetails);
        Task SendResetPasswordEmailAsync(ResetPasswordEmailDto accountDetails);
    }

    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailConfirmationEmailAsync(ConfirmEmailDto accountDetails)
        {
            var client = new SendGridClient(Options.SendGridKey);

            var message = new SendGridMessage();

            message.SetTemplateId("d-882e4b133cae40268364c8a929e55ea9");
            message.SetTemplateData(new ConfirmEmailDto { Username = accountDetails.Username, Url = accountDetails.Url });
            message.SetFrom("andrew.cross11@gmail.com", "Birder Administrator");
            message.AddTo(new EmailAddress(accountDetails.Email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            message.SetClickTracking(false, false);

            return client.SendEmailAsync(message);
        }

        public Task SendChangedAccountEmailConfirmationEmailAsync(ConfirmEmailDto accountDetails)
        {
            var client = new SendGridClient(Options.SendGridKey);

            var message = new SendGridMessage();

            message.SetTemplateId("d-fc1571171e23463bb311870984664506");
            message.SetTemplateData(new ConfirmEmailDto { Username = accountDetails.Username, Url = accountDetails.Url });
            message.SetFrom("andrew.cross11@gmail.com", "Birder Administrator");
            message.AddTo(new EmailAddress(accountDetails.Email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            message.SetClickTracking(false, false);

            return client.SendEmailAsync(message);
        }

        public Task SendResetPasswordEmailAsync(ResetPasswordEmailDto accountDetails)
        {
            var client = new SendGridClient(Options.SendGridKey);

            var message = new SendGridMessage();

            message.SetTemplateId("d-37733c23b2eb4c339a011dfadbd42b91");
            message.SetTemplateData(new ConfirmEmailDto { Username = accountDetails.Username, Url = accountDetails.Url });
            message.SetFrom("andrew.cross11@gmail.com", "Birder Administrator");
            message.AddTo(new EmailAddress(accountDetails.Email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            message.SetClickTracking(false, false);

            return client.SendEmailAsync(message);
        }
    }
}

//public Task SendEmailAsync(string email, string subject, string message, string username, Uri url)
//{
//    return Execute(Options.SendGridKey, subject, message, email, username, url);
//}

//public Task Execute(string apiKey, string subject, string message, string email, string username, Uri url)
//{
//    var client = new SendGridClient(apiKey);

//    var msg = new SendGridMessage();

//    //{
//    //    From = new EmailAddress("Birder@Birder.com", "Birder Administrator"),
//    //    Subject = subject,
//    //    //PlainTextContent = message,
//    //    //HtmlContent = message,
//    //    //
//    //    //TemplateId = "birder-email-confirmation",
//    //};
//    // msg.Subject = subject;
//    msg.SetTemplateId("d-882e4b133cae40268364c8a929e55ea9");
//    //msg.SetTemplateData(new RegisterEmailData { Username = username, Url = url  });
//    msg.SetFrom("andrew.cross11@gmail.com", "Birder Administrator");
//    msg.AddTo(new EmailAddress(email));

//    // Disable click tracking.
//    // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
//    msg.SetClickTracking(false, false);

//    return client.SendEmailAsync(msg);
//}