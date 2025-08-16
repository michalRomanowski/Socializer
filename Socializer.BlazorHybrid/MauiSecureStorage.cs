namespace Socializer.BlazorHybrid;

internal class MauiSecureStorage : Common.Utils.ISecureStorage
{
    public async Task<string?> GetAsync(string key)
    {
        return await SecureStorage.Default.GetAsync(key);
    }

    public async Task SetAsync(string key, string value)
    {
        await SecureStorage.Default.SetAsync(key, value);
    }

    public bool Remove(string key)
    {
        throw new NotImplementedException();
    }

    public void RemoveAll()
    {
        SecureStorage.RemoveAll();
    }
}
