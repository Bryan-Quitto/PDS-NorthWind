namespace NorthWind.Exceptions.Entities.Exceptions;

public class ConcurrencyException : Exception
{
    public IReadOnlyList<string> Entries { get; }

    public ConcurrencyException()
        : base("El registro fue modificado por otra transacción en curso. Por favor, espere a que termine o intente nuevamente.")
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
        : base("El registro fue modificado por otra transacción en curso. Por favor, espere a que termine o intente nuevamente.")
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
