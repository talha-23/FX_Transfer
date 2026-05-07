using System.Threading.Tasks;

namespace FXTransfer.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
    Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
}