using System;
using System.Threading.Tasks;

namespace Birder.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, string username, Uri url);
    }
}
