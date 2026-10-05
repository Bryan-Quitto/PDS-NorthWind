namespace NorthWind.Result.Entities;

public class Result<TSuccess, TError>
{
    public bool HasError { get; }
    public TSuccess? SuccessValue { get; }
    public TError? ErrorValue { get; }

    protected Result(bool hasError, TSuccess? successValue, TError? errorValue)
    {
        HasError = hasError;
        SuccessValue = successValue;
        ErrorValue = errorValue;
    }

    public static Result<TSuccess, TError> Success(TSuccess successValue) => new(false, successValue, default);
    public static Result<TSuccess, TError> Fail(TError errorValue) => new(true, default, errorValue);

    public void HandleResult(Action<TSuccess> onSuccess, Action<TError> onError)
    {
        if (HasError)
        {
            onError(ErrorValue!);
        }
        else
        {
            onSuccess(SuccessValue!);
        }
    }

    public TResult HandleResult<TResult>(Func<TSuccess, TResult> onSuccess, Func<TError, TResult> onError)
    {
        return HasError ? onError(ErrorValue!) : onSuccess(SuccessValue!);
    }
}
