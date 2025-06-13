using System.ComponentModel.DataAnnotations;

namespace Common.Utils;

public static class ValidationExtensions
{
    public static OperationResult<T> Validate<T>(this T dto)
        where T : class
    {
        var context = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        return Validator.TryValidateObject(dto, context, validationResults) ?
            OperationResult<T>.Success(dto) :
            OperationResult<T>.Failure(validationResults.Select(x => x.ErrorMessage.ToString()));
    }
}
