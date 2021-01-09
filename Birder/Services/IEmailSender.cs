using Birder.Templates;
using System;
using System.Threading.Tasks;

namespace Birder.Services
{
    public interface IEmailSender
    {
        // remove 'subject' and 'message' parameters
        // add 'templateId' parameter?
        Task SendEmailAsync(string email, string subject, string message, string username, Uri url);

        Task SendEmailConfirmationemailAsync(ConfirmEmailDto accountDetails);
    }
}
