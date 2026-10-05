namespace NorthWind.Sales.Backend.DataContexts.EFCore.Options;

public class DBOptions
{
    public const string SectionKey = "ConnectionStrings";

    public string NorthWindDB { get; set; } = "Server=(localdb)\\mssqllocaldb;Database=NorthWind;Trusted_Connection=True;MultipleActiveResultSets=true";
    public string NorthWindDomainLogsDB { get; set; } = "Server=(localdb)\\mssqllocaldb;Database=NorthWindDomainLogs;Trusted_Connection=True;MultipleActiveResultSets=true";
}
