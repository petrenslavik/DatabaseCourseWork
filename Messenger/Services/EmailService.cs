using MailKit.Net.Smtp;
using MailKit.Security;
using Messenger.Options;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace Messenger.Services
{
    public class EmailService
    {
        private SmtpConfig SmtpConfig { get; }
        
        public EmailService(IOptions<SmtpConfig> config)
        {
            SmtpConfig = config.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Confirm your registration", SmtpConfig.User));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.Connect(SmtpConfig.Server, SmtpConfig.Port, SecureSocketOptions.Auto);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(SmtpConfig.User, SmtpConfig.Pass);
                client.Send(emailMessage);

                client.Disconnect(true);
            }
        }
    }
}
