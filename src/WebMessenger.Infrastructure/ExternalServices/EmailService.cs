using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using WebMessenger.Core.Interfaces.Services;

namespace WebMessenger.Infrastructure.ExternalServices;

public class EmailSettings
{
  public string SmtpServer { get; set; } = string.Empty;
  public int SmtpPort { get; set; }
  public string SenderEmail { get; set; } = string.Empty;
  public string SenderName { get; set; } = string.Empty;
  public string SmtpUsername { get; set; } = string.Empty;
  public string SmtpPassword { get; set; } = string.Empty;
  public bool EnableSsl { get; set; }
}

public class EmailService(IOptions<EmailSettings> emailSettings) : IEmailService
{
  private readonly EmailSettings _emailSettings = emailSettings.Value;

  public async Task SendEmailVerificationAsync(string email, string token)
  {
    var verificationLink = $"{token}";
    var subject = "Verify your email address";
    var body = $"""
                <h1>Welcome to WebMessenger!</h1>
                <p>Thank you for registering. Please verify your email address by clicking the button below:</p>
                <a href='{verificationLink}' style='display: inline-block; padding: 10px 20px; background-color: #4CAF50; color: white; text-decoration: none; border-radius: 5px;'>Verify Email</a>
                <p>If the button doesn't work, you can copy and paste the following link into your browser:</p>
                <p>{verificationLink}</p>
                <p>The link will expire in 7 days.</p>
                <p>If you did not register for WebMessenger, please ignore this email.</p>
                """;

    await SendEmailAsync(email, subject, body);
  }

  public async Task SendPasswordResetAsync(string email, string token)
  {
    var resetLink = $"{token}";
    var subject = "Reset your password";
    var body = $"""
                <h1>Reset Your Password</h1>
                <p>We received a request to reset your password. Click the button below to create a new password:</p>
                <a href='{resetLink}' style='display: inline-block; padding: 10px 20px; background-color: #4CAF50; color: white; text-decoration: none; border-radius: 5px;'>Reset Password</a>
                <p>If the button doesn't work, you can copy and paste the following link into your browser:</p>
                <p>{resetLink}</p>
                <p>The link will expire in 24 hours.</p>
                <p>If you did not request a password reset, please ignore this email.</p>
                """;

    await SendEmailAsync(email, subject, body);
  }

  private async Task SendEmailAsync(string to, string subject, string htmlBody)
  {
    using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
    client.UseDefaultCredentials = false;
    client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
    client.EnableSsl = _emailSettings.EnableSsl;
    
    using var message = new MailMessage();
    message.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
    message.To.Add(to);
    message.Subject = subject;
    message.Body = htmlBody;
    message.IsBodyHtml = true;
    
    await client.SendMailAsync(message);
  }
}