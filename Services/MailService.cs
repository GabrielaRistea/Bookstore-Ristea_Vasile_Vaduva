using Bookstore.Services.Interfaces;
using Bookstore.Utils;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace Bookstore.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;
        public MailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task SendEmailAsync(MailMessage email)
        {
            using var smtpClient = new SmtpClient
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Host = _settings.Host,
                Port = _settings.Port,
                Credentials = new NetworkCredential
                {
                    UserName = _settings.Username,
                    Password = _settings.AppPassword
                },
            };

            if (email.From == null)
            {
                email.From = new MailAddress(_settings.Username, _settings.DisplayName);
            }

            await smtpClient.SendMailAsync(email);
        }

    }
}
