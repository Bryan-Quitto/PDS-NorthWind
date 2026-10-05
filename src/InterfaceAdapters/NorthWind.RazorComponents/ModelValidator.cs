namespace NorthWind.RazorComponents;

public class ModelValidator<T> : ComponentBase, IDisposable
{
    private EditContext? _previousEditContext;
    private ValidationMessageStore? _messageStore;

    [CascadingParameter]
    public EditContext? CurrentEditContext { get; set; }

    [Inject]
    public IModelValidatorHub<T> ValidatorHub { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException($"{nameof(ModelValidator<T>)} requiere un parámetro en cascada {nameof(EditContext)}.");
        }

        HookEditContext(CurrentEditContext);
    }

    protected override void OnParametersSet()
    {
        if (CurrentEditContext != _previousEditContext)
        {
            if (_previousEditContext != null)
            {
                UnhookEditContext(_previousEditContext);
            }

            if (CurrentEditContext != null)
            {
                HookEditContext(CurrentEditContext);
            }
        }
    }

    private void HookEditContext(EditContext editContext)
    {
        _previousEditContext = editContext;
        _messageStore = new ValidationMessageStore(editContext);

        editContext.OnValidationRequested += HandleValidationRequested;
        editContext.OnFieldChanged += HandleFieldChanged;
    }

    private void UnhookEditContext(EditContext editContext)
    {
        editContext.OnValidationRequested -= HandleValidationRequested;
        editContext.OnFieldChanged -= HandleFieldChanged;
        _messageStore?.Clear();
    }

    private async void HandleValidationRequested(object? sender, ValidationRequestedEventArgs args)
    {
        if (CurrentEditContext == null || _messageStore == null) return;

        _messageStore.Clear();

        if (CurrentEditContext.Model is T model)
        {
            var errors = await ValidatorHub.ValidateAsync(model);
            foreach (var error in errors)
            {
                var fieldIdentifier = ResolveFieldIdentifier(model, error.PropertyName);
                _messageStore.Add(fieldIdentifier, error.ErrorMessage);
            }
        }

        CurrentEditContext.NotifyValidationStateChanged();
    }

    private async void HandleFieldChanged(object? sender, FieldChangedEventArgs args)
    {
        if (CurrentEditContext == null || _messageStore == null) return;

        _messageStore.Clear(args.FieldIdentifier);

        if (CurrentEditContext.Model is T model)
        {
            var errors = await ValidatorHub.ValidateAsync(model);
            foreach (var error in errors)
            {
                var fieldIdentifier = ResolveFieldIdentifier(model, error.PropertyName);
                if (fieldIdentifier.Equals(args.FieldIdentifier))
                {
                    _messageStore.Add(fieldIdentifier, error.ErrorMessage);
                }
            }
        }

        CurrentEditContext.NotifyValidationStateChanged();
    }

    private static FieldIdentifier ResolveFieldIdentifier(object rootModel, string propertyPath)
    {
        if (string.IsNullOrWhiteSpace(propertyPath))
        {
            return new FieldIdentifier(rootModel, string.Empty);
        }

        var parts = propertyPath.Split('.');
        object currentTarget = rootModel;

        for (var i = 0; i < parts.Length - 1; i++)
        {
            var part = parts[i];

            if (part.Contains('['))
            {
                var propName = part[..part.IndexOf('[')];
                var indexStr = part[(part.IndexOf('[') + 1)..part.IndexOf(']')];

                var collectionProp = currentTarget.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (collectionProp == null) return new FieldIdentifier(rootModel, propertyPath);

                var collectionValue = collectionProp.GetValue(currentTarget);
                if (collectionValue is System.Collections.IList list && int.TryParse(indexStr, out var index))
                {
                    if (index >= 0 && index < list.Count)
                    {
                        var item = list[index];
                        if (item != null)
                        {
                            currentTarget = item;
                            continue;
                        }
                    }
                }
                return new FieldIdentifier(rootModel, propertyPath);
            }

            var prop = currentTarget.GetType().GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop == null) return new FieldIdentifier(rootModel, propertyPath);

            var nextTarget = prop.GetValue(currentTarget);
            if (nextTarget == null) return new FieldIdentifier(rootModel, propertyPath);

            currentTarget = nextTarget;
        }

        var lastPart = parts[^1];
        if (lastPart.Contains('['))
        {
            var propName = lastPart[..lastPart.IndexOf('[')];
            return new FieldIdentifier(currentTarget, propName);
        }

        return new FieldIdentifier(currentTarget, lastPart);
    }

    public void Dispose()
    {
        if (_previousEditContext != null)
        {
            UnhookEditContext(_previousEditContext);
            _previousEditContext = null;
        }
        GC.SuppressFinalize(this);
    }
}
