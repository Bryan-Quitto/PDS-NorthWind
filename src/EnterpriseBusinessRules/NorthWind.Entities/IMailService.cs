namespace NorthWind.Entities;

public interface IMailService
{
    Task SendMailToAdministrator(string subject, string body, CancellationToken cancellationToken = default);
}
