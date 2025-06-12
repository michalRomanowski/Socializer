namespace Common.Utils;

public class OperationResult<TResult>
{
    public TResult? Result { get; private set; }

    public IEnumerable<string> Errors { get; private set; } = [];

    public string ErrorMessage => Errors.Any(x => string.IsNullOrEmpty(x) == false) ?
        string.Join(Environment.NewLine, Errors) :
        "Unspecified error.";

    public bool IsSuccess => !Errors.Any();

    public static OperationResult<TResult> Success(TResult result)
    {
        return new OperationResult<TResult>()
        {
            Result = result,
            Errors = []
        };
    }

    public static OperationResult<TResult> Failure(Exception ex)
    {
        return new OperationResult<TResult>()
        {
            Errors = [ex.Message]
        };
    }

    public static OperationResult<TResult> Failure(string error)
    {
        return new OperationResult<TResult>()
        {
            Errors = [error]
        };
    }

    public static OperationResult<TResult> Failure(IEnumerable<string> errors)
    {
        return new OperationResult<TResult>()
        {
            Errors = errors
        };
    }
}
