namespace MauiStrides.Services
{
    public interface ITokenService
    {
        Task<string> GetValidAccessTokenAsync();
        Task StoreTokensAsync(string accessToken, string refreshToken, long expiresAt);
        Task<bool> HasStoredTokensAsync();
        Task ClearTokensAsync();
    }
}