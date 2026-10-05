namespace NorthWind.Sales.Backend.SmtpGateways.Options;

public class SmtpOptions
{
    public const string SectionKey = "Smtp";

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 25;
    public string From { get; set; } = "noreply@northwind.com";
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public bool EnableSsl { get; set; } = false;
}
