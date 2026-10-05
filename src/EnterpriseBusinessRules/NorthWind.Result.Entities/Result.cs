namespace NorthWind.Result.Entities;

public class Result<TError>
{
    public bool HasError { get; }
    public TError? ErrorValue { get; }

    protected Result(bool hasError, TError? errorValue)
    {
        HasError = hasError;
        ErrorValue = errorValue;
    }

    public static Result<TError> Success() => new(false, default);
    public static Result<TError> Fail(TError error) => new(true, error);

    public void HandleResult(Action onSuccess, Action<TError> onError)
    {
        if (HasError)
        {
            onError(ErrorValue!);
        }
        else
        {
            onSuccess();
        }
    }

    public TResult HandleResult<TResult>(Func<TResult> onSuccess, Func<TError, TResult> onError)
    {
        return HasError ? onError(ErrorValue!) : onSuccess();
    }
}
