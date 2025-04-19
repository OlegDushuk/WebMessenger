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
    var verificationLink = $"https://localhost:7151/activation-result/{token}";
    var subject = "Verify your email address";
    var body = $"""
                <h1>Ласкаво просимо до WebMessenger!</h1>
                <p>Дякуємо за реєстрацію. Підтвердьте свою електронну адресу, натиснувши кнопку нижче:</p>
                <a href='{verificationLink}' style='display: inline-block; padding: 10px 20px; background-color: #4CAF50; color: white; text-decoration: none; border-radius: 5px;'>Verify Email</a>
                <p>Якщо кнопка не працює, ви можете скопіювати та вставити це посилання у свій браузер:</p>
                <p>{verificationLink}</p>
                <p>Термін дії посилання закінчується через 2 днів.</p>
                <p>Якщо ви не зареєструвалися в WebMessenger, проігноруйте цей електронний лист.</p>
                """;

    await SendEmailAsync(email, subject, body);
  }

  public async Task SendPasswordResetAsync(string email, string token)
  {
    var resetLink = $"https://localhost:7151/reset-password/{token}";
    var subject = "Reset your password";
    var body = $"""
                <h1>Скинути пароль</h1>
                <p>Ми отримали запит на скидання вашого пароля. Натисніть кнопку нижче, щоб створити новий пароль:</p>
                <a href='{resetLink}' style='display: inline-block; padding: 10px 20px; background-color: #4CAF50; color: white; text-decoration: none; border-radius: 5px;'>Скинути пароль</a>
                <p>Якщо кнопка не працює, ви можете скопіювати та вставити це посилання у свій браузер:</p>
                <p>{resetLink}</p>
                <p>Термін дії посилання закінчиться через 2 дні.</p>
                <p>Якщо ви не надсилали запит на скидання пароля, проігноруйте цей електронний лист.</p>
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