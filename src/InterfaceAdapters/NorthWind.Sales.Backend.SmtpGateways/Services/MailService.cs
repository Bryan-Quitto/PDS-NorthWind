using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using NorthWind.Entities;
using NorthWind.Sales.Backend.SmtpGateways.Options;

namespace NorthWind.Sales.Backend.SmtpGateways.Services;

public class MailService(IOptions<SmtpOptions> options) : IMailService
{
    private readonly SmtpOptions smtpOptions = options.Value;

    public async Task SendMailToAdministrator(string subject, string body, CancellationToken cancellationToken = default)
    {
        using var client = new SmtpClient(smtpOptions.Host, smtpOptions.Port)
        {
            EnableSsl = smtpOptions.EnableSsl
        };

        if (!string.IsNullOrEmpty(smtpOptions.UserName) && !string.IsNullOrEmpty(smtpOptions.Password))
        {
            client.Credentials = new NetworkCredential(smtpOptions.UserName, smtpOptions.Password);
        }

        using var message = new MailMessage(smtpOptions.From, smtpOptions.AdministratorEmail, subject, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message, cancellationToken);
    }
}
