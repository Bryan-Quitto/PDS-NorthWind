namespace NorthWind.Validation.Entities.ValueObjects;

public record ValidationError(string PropertyName, string ErrorMessage);
