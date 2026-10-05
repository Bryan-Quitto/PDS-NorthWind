namespace NorthWind.Entities;

public interface IMailService
{
    Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
