using Common.Utils;

namespace Common.Client;

public interface IClient
{
    Task<ClientOperationResult<TDto>> GetAsync<TDto>(string urlPath);
    Task<ClientOperationResult<TDto>> PostAsync<TDto>(string urlPath, TDto dto);
    Task<OperationResult<bool>> LoginAsync(string username, string password);
    /// <summary>
    /// Login from saved tokens
    /// </summary>
    /// <returns></returns>
    Task<OperationResult<bool>> LoginAsync();
}
