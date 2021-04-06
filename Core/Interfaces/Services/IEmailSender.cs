using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body, string from = null);
    }
}
