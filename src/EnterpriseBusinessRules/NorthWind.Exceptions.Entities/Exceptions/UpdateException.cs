namespace NorthWind.Exceptions.Entities.Exceptions;

public class UpdateException : Exception
{
    public IReadOnlyList<string> Entries { get; }

    public UpdateException()
        : base("Ocurrió un error al intentar actualizar la base de datos.")
    {
        Entries = [];
    }

    public UpdateException(string message)
        : base(message)
    {
        Entries = [];
    }

    public UpdateException(string message, Exception innerException)
        : base(message, innerException)
    {
        Entries = [];
    }

    public UpdateException(IEnumerable<string> entries)
        : base("Ocurrió un error al intentar actualizar la base de datos.")
    {
        Entries = [.. entries];
    }

    public UpdateException(string message, IEnumerable<string> entries)
        : base(message)
    {
        Entries = [.. entries];
    }

    public UpdateException(string message, Exception innerException, IEnumerable<string> entries)
        : base(message, innerException)
    {
        Entries = [.. entries];
    }
}
