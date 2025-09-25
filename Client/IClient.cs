using Common.Utils;

namespace Common.Client;

public interface IClient
{
    Task<OperationResult<TDto>> GetAsync<TDto>(string urlPath);
    Task<OperationResult<TDto>> PostAsync<TDto>(string urlPath, TDto dto);
    Task DeleteAsync(string urlPath);
    Task<OperationResult<bool>> LoginAsync(string username, string password);
    /// <summary>
    /// Login from saved tokens
    /// </summary>
    /// <returns></returns>
    Task<OperationResult<bool>> LoginAsync();
}
