namespace NorthWind.Exceptions.Entities.Exceptions;

public class ConcurrencyException : Exception
{
    public IReadOnlyList<string> Entries { get; }

    public ConcurrencyException()
        : base("A concurrency conflict occurred. The data has been modified by another process.")
    {
        Entries = [];
    }

    public ConcurrencyException(string message)
        : base(message)
    {
        Entries = [];
    }

    public ConcurrencyException(string message, Exception innerException)
        : base(message, innerException)
    {
        Entries = [];
    }

    public ConcurrencyException(IEnumerable<string> entries)
        : base("A concurrency conflict occurred. The data has been modified by another process.")
    {
        Entries = [.. entries];
    }

    public ConcurrencyException(string message, IEnumerable<string> entries)
        : base(message)
    {
        Entries = [.. entries];
    }

    public ConcurrencyException(string message, Exception innerException, IEnumerable<string> entries)
        : base(message, innerException)
    {
        Entries = [.. entries];
    }
}
