using GarageManagement.Server.Model;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

using System.Net.Mail;

namespace GarageManagement.Server
{
    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; }
    }



    public class EmailService
    {
        private readonly SmtpSettings _settings;

        public EmailService(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(EmailRequest mailRequest)
        {
            if(string.IsNullOrEmpty(mailRequest.ToEmail))
                 throw new ArgumentNullException(nameof(mailRequest.ToEmail), "Recipient email cannot be null");

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_settings.FromEmail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = string.IsNullOrEmpty(mailRequest.Subject) ? "Password Reset" : mailRequest.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = string.IsNullOrEmpty(mailRequest.Body)
                    ? "<p>Please click the link to reset your password.</p>"
                    : mailRequest.Body
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_settings.Server, _settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.User, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }

}
