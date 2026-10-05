namespace NorthWind.Sales.Backend.DataContexts.EFCore.Options;

public class DBOptions
{
    public const string SectionKey = "DBOptions";

    public string ConnectionString { get; set; } = "Server=localhost;Database=NorthWindDB;Trusted_Connection=True;TrustServerCertificate=True;";
    public string DomainLogsConnectionString { get; set; } = "Server=localhost;Database=NorthWindLogsDB;Trusted_Connection=True;TrustServerCertificate=True;";
}
