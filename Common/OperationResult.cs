using System.Net.Http.Json;
using System.Text.Json;

namespace Common.Utils;

public class OperationResult<TResult>
{
    public TResult? Result { get; private set; }

    public IEnumerable<string> Errors { get; private set; } = [];

    public string ErrorMessage => Errors.Any(x => string.IsNullOrEmpty(x) == false) ?
        string.Join(Environment.NewLine, Errors) :
        "Unspecified error.";

    public bool IsSuccess => !Errors.Any();

    private readonly static JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true
    };

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

    public static async Task<OperationResult<TResult>> FromHttpResponseAsync(HttpResponseMessage httpResponseMessage)
    {
        try
        {
            var statusCode = (int)httpResponseMessage.StatusCode;

            if (statusCode >= 200 && statusCode <= 299)
            {
                var deserializedResponse = await httpResponseMessage.Content.ReadFromJsonAsync<TResult>(options);
                return OperationResult<TResult>.Success(deserializedResponse);
            }

            return OperationResult<TResult>.Failure(await httpResponseMessage.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            // TODO: log exception only, for now will expose ex.message in response to see in UI for early stage
            return OperationResult<TResult>.Failure(
                $"Error processing response. Message: {ex.Message} InnerMessage: {ex.InnerException?.Message ?? "NULL"}");
        }
    }
}
