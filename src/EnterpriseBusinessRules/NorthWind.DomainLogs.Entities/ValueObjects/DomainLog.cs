namespace NorthWind.DomainLogs.Entities.ValueObjects;

public class DomainLog
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string Information { get; set; } = string.Empty;

    public DomainLog() { }

    public DomainLog(string information)
    {
        CreatedDate = DateTime.UtcNow;
        Information = information;
    }
}
