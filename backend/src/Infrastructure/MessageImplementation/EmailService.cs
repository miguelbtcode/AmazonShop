using System.Net;
using System.Net.Mail;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ecommerce.Infrastructure.MessageImplementation;

public class EmailService : IEmailService
{
    public GmailSettings _gmailSettings { get; }
    public ILogger<EmailService> _logger { get; set; }

    public EmailService(IOptions<GmailSettings> gmailSettings, ILogger<EmailService> logger)
    {
        _gmailSettings = gmailSettings.Value;
        _logger = logger;
    }

    public async Task<bool> SendEmail(EmailMessage email, string token)
    {

        var fromEmail = _gmailSettings.Username;
        var password = _gmailSettings.Password;

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = _gmailSettings.Port,
            Credentials = new NetworkCredential(fromEmail, password),
            EnableSsl = true
        };

        var message = new MailMessage
        {
            From = new MailAddress(fromEmail!),
            Subject = email.Subject,
            Body = email.Body,
            IsBodyHtml = true
        };

        message.To.Add(new MailAddress(email.To!));

        try
        {
            // Enviar el correo de forma asíncrona
            var userToken = new CancellationToken(); // Puedes pasar cualquier objeto como userToken
            await smtpClient.SendMailAsync(message, userToken);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("El email no pudo enviarse, existen errores", e);
            return false;
        }
        finally
        {
            smtpClient.Dispose();
            message.Dispose();
        }
    }
}