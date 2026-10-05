using NorthWind.Validation.Entities.ValueObjects;

namespace NorthWind.Exceptions.Entities.Exceptions;

public class ValidationException : Exception
{
    public IReadOnlyList<ValidationError> Errors { get; }

    public ValidationException()
        : base("Se presentaron uno o más errores de validación.")
    {
        Errors = [];
    }

    public ValidationException(string message)
        : base(message)
    {
        Errors = [];
    }

    public ValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
        Errors = [];
    }

    public ValidationException(IEnumerable<ValidationError> errors)
        : base("Se presentaron uno o más errores de validación.")
    {
        Errors = [.. errors];
    }

    public ValidationException(string propertyName, string errorMessage)
        : base("Se presentaron uno o más errores de validación.")
    {
        Errors = [new ValidationError(propertyName, errorMessage)];
    }
}
