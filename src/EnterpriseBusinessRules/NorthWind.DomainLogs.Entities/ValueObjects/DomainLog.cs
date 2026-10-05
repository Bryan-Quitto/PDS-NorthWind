namespace NorthWind.DomainLogs.Entities.ValueObjects;

public class DomainLog
{
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public string Action { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;

    public DomainLog() { }

    public DomainLog(string action, string description, string user)
    {
        Created = DateTime.UtcNow;
        Action = action;
        Description = description;
        User = user;
    }
}
