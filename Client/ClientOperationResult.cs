using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Common.Client;

public class ClientOperationResult<TResult>  // TODO: Extract non generic version so don't have to do ClientOperationResult<bool>
{
    public TResult? Result { get; private set; }

    private IEnumerable<string> errors { get; set; } = [];
    public string ErrorMessage => errors.Any(x => string.IsNullOrEmpty(x) == false) ?
            string.Join(Environment.NewLine, errors) :
            "Unspecified error.";

    public HttpStatusCode StatusCode { get; private set; }
    public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;

    private readonly static JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<ClientOperationResult<TResult>> FromResponseAsync(HttpResponseMessage response)
    {
        try
        {
            if (response.IsSuccessStatusCode)
            {
                var deserializedResponse = await response.Content.ReadFromJsonAsync<TResult>(options);
                return new ClientOperationResult<TResult>()
                {
                    StatusCode = response.StatusCode,
                    Result = deserializedResponse
                };
            }

            return new ClientOperationResult<TResult>()
            {
                StatusCode = response.StatusCode,
                errors = [await response.Content.ReadAsStringAsync()]
            };
        }
        catch (Exception ex)
        {
            // TODO: log exception only, for now will expose ex.message in response to see in UI for early stage
            return new ClientOperationResult<TResult>()
            {
                StatusCode = HttpStatusCode.BadRequest,
                errors = [$"Error processing response. Message: {ex.Message} InnerMessage: {ex.InnerException?.Message ?? "NULL"}"]
            };
        }
    }

    public static ClientOperationResult<TResult> FromException(Exception ex)
    {
        return new ClientOperationResult<TResult>()
        {
            StatusCode = HttpStatusCode.InternalServerError,
            errors = [$"Exception occurred: {ex.Message} InnerMessage: " + ex.InnerException?.Message + ex.InnerException?.InnerException?.Message]
        };
    }

    public static ClientOperationResult<TResult> FromErrors(IEnumerable<string> errors)
    {
        return new ClientOperationResult<TResult>()
        {
            StatusCode = HttpStatusCode.BadRequest, // TODO: consider using a more specific status code
            errors = errors
        };
    }
}
