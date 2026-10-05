namespace NorthWind.Sales.Backend.DataContexts.EFCore.Options;

public class DBOptions
{
    public const string SectionKey = "DBOptions";

    public string ConnectionString { get; set; } = "Server=(localdb)\\mssqllocaldb;Database=NorthWind;Trusted_Connection=True;MultipleActiveResultSets=true";
    public string DomainLogsConnectionString { get; set; } = "Server=(localdb)\\mssqllocaldb;Database=NorthWindDomainLogs;Trusted_Connection=True;MultipleActiveResultSets=true";
}
