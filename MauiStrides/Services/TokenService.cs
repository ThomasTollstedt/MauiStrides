using MauiStrides.Models;
using System.Net.Http.Json;

namespace MauiStrides.Services
{
    public class TokenService : ITokenService
    {
        private const string AccessTokenKey = "strava_access_token";
        private const string RefreshTokenKey = "strava_refresh_token";
        private const string ExpiresAtKey = "strava_expires_at";

        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;


        public TokenService(HttpClient httpClient, StravaConfiguration configuration)
        {
            _httpClient = httpClient;
            _clientId = configuration.ClientId;
            _clientSecret = configuration.ClientSecret;

            if (string.IsNullOrEmpty(_clientId) || string.IsNullOrEmpty(_clientSecret))
            {
                throw new InvalidOperationException("Strava ClientId and ClientSecret must be configured in appsettings.json");
            }
        }

        // Fixa refreshToken eller refreshan automatiskt när token är på väg att gå ut.
        public async Task<string> GetValidAccessTokenAsync()
        {
            var accessToken = await SecureStorage.GetAsync(AccessTokenKey);
            var expiresAtStr = await SecureStorage.GetAsync(ExpiresAtKey);

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(expiresAtStr))
            {
                throw new InvalidOperationException("No stored tokens found. User needs to authenticate.");
            }

            var expiresAt = long.Parse(expiresAtStr);
            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expiresAt);

            // Refreshar om utgånget eller om det går ut inom 5 minuter
            if (DateTimeOffset.UtcNow.AddMinutes(5) >= expirationTime)
            {
                return await RefreshAccessTokenAsync();
            }

            return accessToken;
        }

        //Lagra via SecureStorage
        public async Task StoreTokensAsync(string accessToken, string refreshToken, long expiresAt)
        {
            await SecureStorage.SetAsync(AccessTokenKey, accessToken);
            await SecureStorage.SetAsync(RefreshTokenKey, refreshToken);
            await SecureStorage.SetAsync(ExpiresAtKey, expiresAt.ToString());
        }


        // Checks if tokens are stored

        public async Task<bool> HasStoredTokensAsync()
        {
            var accessToken = await SecureStorage.GetAsync(AccessTokenKey);
            return !string.IsNullOrEmpty(accessToken);
        }

        // Cleara tokens vid utloggning
        public async Task ClearTokensAsync()
        {
            SecureStorage.Remove(AccessTokenKey);
            SecureStorage.Remove(RefreshTokenKey);
            SecureStorage.Remove(ExpiresAtKey);
            await Task.CompletedTask;
        }


        // LoginAsync
        public async Task<string> LoginAsync(string authorizationCode)
        {
            // Prepare the token request
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret),
                new KeyValuePair<string, string>("code", authorizationCode),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            });
            var response = await _httpClient.PostAsync("https://www.strava.com/oauth/token", requestContent);
            response.EnsureSuccessStatusCode();
            var tokenResponse = await response.Content.ReadFromJsonAsync<StravaTokenResponse>();
            if (tokenResponse == null)
            {
                throw new InvalidOperationException("Failed to deserialize token response");
            }
            // Store the tokens
            await StoreTokensAsync(
                tokenResponse.AccessToken,
                tokenResponse.RefreshToken,
                tokenResponse.ExpiresAt);
            return tokenResponse.AccessToken;
        }


        private async Task<string> RefreshAccessTokenAsync()
        {
            var refreshToken = await SecureStorage.GetAsync(RefreshTokenKey);

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new InvalidOperationException("No refresh token found. User needs to re-authenticate.");
            }

            // Prepare the refresh request
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            });

            try
            {
                var response = await _httpClient.PostAsync("https://www.strava.com/oauth/token", requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Token refresh failed: {response.StatusCode}. Details: {error}");
                }

                var tokenResponse = await response.Content.ReadFromJsonAsync<StravaTokenResponse>();

                if (tokenResponse == null)
                {
                    throw new InvalidOperationException("Failed to deserialize token response");
                }

                // Store the NEW tokens (Strava rotates refresh tokens!)
                await StoreTokensAsync(
                    tokenResponse.AccessToken,
                    tokenResponse.RefreshToken,
                    tokenResponse.ExpiresAt);

                return tokenResponse.AccessToken;
            }
            catch (HttpRequestException ex)
            {
                // Token refresh failed - user needs to re-authenticate
                await ClearTokensAsync();
                throw new InvalidOperationException("Token refresh failed. Please log in again.", ex);
            }
        }
    }
}