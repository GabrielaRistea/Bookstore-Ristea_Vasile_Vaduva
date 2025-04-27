using System.Net.Mail;

namespace Bookstore.Services.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailMessage email);
    }
}
