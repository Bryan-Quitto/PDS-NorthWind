namespace NorthWind.Validation.Entities.Interfaces;

public interface ICollectionValidationRules<T, TElement>
{
    ICollectionValidationRules<T, TElement> NotEmpty(string errorMessage);
    ICollectionValidationRules<T, TElement> SetValidator(IModelValidator<TElement> validator);
}
