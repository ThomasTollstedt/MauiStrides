using MauiStrides.Models;
using MauiStrides.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MauiStrides.Client
{
    public class StravaApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;
        private const string BaseUrl = "https://www.strava.com/api/v3";

        public StravaApiClient(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _tokenService = tokenService;
        }

        /// <summary>
        /// Gets all activities for the authenticated athlete (token handled automatically)
        /// </summary>
        public async Task<List<Activity>> GetActivitiesAsync()
        {
            await SetAuthorizationHeaderAsync();
            return await GetAsync<List<Activity>>("/athlete/activities");
        }

        /// <summary>
        /// Gets the authenticated athlete's profile (token handled automatically)
        /// </summary>
        public async Task<AthleteProfile> GetAthleteProfileAsync()
        {
            await SetAuthorizationHeaderAsync();
            return await GetAsync<AthleteProfile>("/athlete");
        }

        /// <summary>
        /// Gets detailed information about a specific activity (token handled automatically)
        /// </summary>
        public async Task<Activity> GetActivityDetailsAsync(long activityId)
        {
            await SetAuthorizationHeaderAsync();
            return await GetAsync<Activity>($"/activities/{activityId}");
        }

        // ============================================
        // PRIVATE HELPER METHODS
        // ============================================

        /// <summary>
        /// Automatically gets valid token and sets Authorization header
        /// Handles token refresh transparently
        /// </summary>
        private async Task SetAuthorizationHeaderAsync()
        {
            var accessToken = await _tokenService.GetValidAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", accessToken);
        }

        /// <summary>
        /// Generic GET request helper (DRY principle)
        /// </summary>
        private async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Strava API error: {response.StatusCode}. Details: {errorDetails}");
            }

            var result = await response.Content.ReadFromJsonAsync<T>();
            
            return result ?? throw new InvalidOperationException(
                $"Failed to deserialize response from {endpoint}");
        }
    }
}
