using System.Threading.Tasks;
using Core.Interfaces.Services;
using Infrastructure.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly EmailSettings _emailSettings;

        public EmailSender(ILogger<EmailSender> logger, IOptions<EmailSettings> emailSettings)
        {
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }
        
        public async Task SendEmailAsync(string to, string subject, string body, string from = null)
        {
            from ??= _emailSettings.EmailFrom;
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) {Text = body};
            
            using var emailClient = new SmtpClient();
            await emailClient.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await emailClient.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
            await emailClient.SendAsync(email);
            _logger.LogWarning("Sending email to {To} from {From} with subject {Subject}", to, from, subject);
            await emailClient.DisconnectAsync(true);
        }
    }
}
