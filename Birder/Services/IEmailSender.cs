using Birder.Templates;
using System;
using System.Threading.Tasks;

namespace Birder.Services
{
    public interface IEmailSender
    {
        Task SendEmailConfirmationEmailAsync(ConfirmEmailDto accountDetails);
        Task SendChangedAccountEmailConfirmationEmailAsync(ConfirmEmailDto accountDetails);
        Task SendResetPasswordEmailAsync(ResetPasswordEmailDto accountDetails);
    }
}
