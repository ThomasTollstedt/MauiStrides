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
        //private const string BaseUrl = "https://www.strava.com/api/v3/";

        public StravaApiClient(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<List<ActivityDTO>> GetActivitiesAsync()
        {
            await SetAuthorizationHeaderAsync();
            return await GetAsync<List<ActivityDTO>>("athlete/activities");
        }

        //overload för pagineringen
        public async Task<List<ActivityDTO>> GetActivitiesAsync(int page, int perPage)
        {
            await SetAuthorizationHeaderAsync();

            string url = $"athlete/activities?page={page}&per_page={perPage}";
           
            return await GetAsync<List<ActivityDTO>>(url);
        }

        
        public async Task<AthleteProfileDTO> GetAthleteProfileAsync()
        {
            await SetAuthorizationHeaderAsync();
            return await GetAsync<AthleteProfileDTO>("athlete");
        }

      
        public async Task<ActivityDTO> GetActivityDetailsAsync(long activityId)
        {
            await SetAuthorizationHeaderAsync();
            return await GetAsync<ActivityDTO>($"activities/{activityId}");
        }

     // Sätts vid varje anrop, Delegating Handler att föredra. 
        private async Task SetAuthorizationHeaderAsync()
        {
            var accessToken = await _tokenService.GetValidAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", accessToken);
        }

        
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
