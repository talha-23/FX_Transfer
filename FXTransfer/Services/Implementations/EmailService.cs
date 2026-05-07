using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FXTransfer.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace FXTransfer.Services.Implementations;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    // For demo purposes - In production, use actual SMTP settings
    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            // For demo, we'll just log the email
            _logger.LogInformation($"EMAIL TO: {toEmail}");
            _logger.LogInformation($"SUBJECT: {subject}");
            _logger.LogInformation($"BODY: {body}");

            // In production, uncomment and configure SMTP:
            /*
            using var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("your-email@gmail.com", "your-password");
            
            var mailMessage = new MailMessage
            {
                From = new MailAddress("noreply@fxtransfer.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);
            
            await client.SendMailAsync(mailMessage);
            */

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send email to {toEmail}");
            throw;
        }
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
    {
        var subject = "Password Reset Request - FXTransfer";
        var body = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    .container {{ max-width: 500px; margin: 0 auto; padding: 20px; }}
                    .header {{ background: linear-gradient(135deg, #00d2ff, #f093fb); padding: 20px; text-align: center; color: white; border-radius: 10px 10px 0 0; }}
                    .content {{ background: #1a1a3e; padding: 30px; border-radius: 0 0 10px 10px; color: white; }}
                    .button {{ background: linear-gradient(135deg, #00d2ff, #3a7bd5); color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block; }}
                    .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #666; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>🔐 FXTransfer</h2>
                        <p>Password Reset Request</p>
                    </div>
                    <div class='content'>
                        <p>Hello,</p>
                        <p>We received a request to reset your password for your FXTransfer account.</p>
                        <p>Click the button below to reset your password:</p>
                        <p style='text-align: center;'>
                            <a href='{resetLink}' class='button'>Reset Password</a>
                        </p>
                        <p>If you didn't request this, please ignore this email.</p>
                        <p>This link will expire in 1 hour.</p>
                        <hr />
                        <small>FXTransfer - Secure Currency Exchange</small>
                    </div>
                </div>
            </body>
            </html>";

        await SendEmailAsync(toEmail, subject, body);
    }
}