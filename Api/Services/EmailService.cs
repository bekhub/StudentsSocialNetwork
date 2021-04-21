using System.Threading.Tasks;
using Core.Interfaces.Services;

namespace Api.Services
{
    public class EmailService
    {
        private readonly IEmailSender _emailSender;

        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public Task SendVerificationEmailAsync(string email, string verificationToken, string host)
        {
            string message;
            if (!string.IsNullOrEmpty(host))
            {
                var verifyUrl = $"{host}/api/verify-email?token={verificationToken}";
                message = $@"<p>Please <a href='{verifyUrl}'>click here</a> to verify your email address.</p>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address with the 
                             <code>/api/verify-email</code> api route:</p>
                             <p><code>{verificationToken}</code></p>";
            }

            return _emailSender.SendEmailAsync(
                to: email,
                subject: "Students Social Network - Verify Email",
                body: $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {message}");
        }

        public Task SendAlreadyRegisteredEmailAsync(string email, string host)
        {
            string message;
            if (!string.IsNullOrEmpty(host))
                message = $@"<p>If you don't know your password please visit the 
                             <a href='{host}/api/forgot-password'>forgot password</a> page.</p>";
            else
                message = "<p>If you don't know your password you can reset it via the " +
                          "<code>/api/forgot-password</code> api route.</p>";

            return _emailSender.SendEmailAsync(
                to: email,
                subject: "Students Social Network - Email Already Registered",
                body: $@"<h4>Email Already Registered</h4>
                         <p>Your email <strong>{email}</strong> is already registered.</p>
                         {message}");
        }

        public Task SendPasswordResetEmailAsync(string email, string resetToken, string host)
        {
            string message;
            if (!string.IsNullOrEmpty(host))
            {
                var resetUrl = $"{host}/api/reset-password?token={resetToken}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                             <p><a href='{resetUrl}'>{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to reset your password with the 
                             <code>/api/reset-password</code> api route:</p>
                             <p><code>{resetToken}</code></p>";
            }

            return _emailSender.SendEmailAsync(
                to: email,
                subject: "Students Social Network - Reset Password",
                body: $@"<h4>Reset Password Email</h4>
                         {message}"
            );
        }
    }
}
