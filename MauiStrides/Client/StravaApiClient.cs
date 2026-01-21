
using MauiStrides.Models;

using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace MauiStrides.Client
{
    public class StravaApiClient
    {
        private readonly HttpClient _httpClient;
        public StravaApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Activity>> GetActivitiesAsync(string accessToken, string? type = null)
        {
            // Implement the logic to call Strava API and fetch activities of the specified type
            HttpResponseMessage response = await SendGetRequestAsync("https://www.strava.com/api/v3/athlete/activities", accessToken);
          
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                var allActivities = System.Text.Json.JsonSerializer.Deserialize<List<Activity>>(json);
                return allActivities;
            }
            else 
            {
                var errorDetails = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Fel statuskod: {response.StatusCode} info: {errorDetails}");
            }
        }

        public async Task<AthleteProfile> GetAthleteProfileAsync(string accessToken)
        {
            // Implement the logic to call Strava API and fetch athlete profile
            HttpResponseMessage response = await SendGetRequestAsync("https://www.strava.com/api/v3/athlete", accessToken);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                var athleteProfile = System.Text.Json.JsonSerializer.Deserialize<AthleteProfile>(json);
                return athleteProfile;
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Fel statuskod: {response.StatusCode} info: {errorDetails}");
            }

        }

       

        public async Task<Activity> GetActivityDetailsAsync(string accessToken, long activityId)
        {
            
            HttpResponseMessage response = await SendGetRequestAsync($"https://www.strava.com/api/v3/activities/{activityId}", accessToken);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                var selectActivity = System.Text.Json.JsonSerializer.Deserialize<Activity>(json);
                return selectActivity;
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Fel statuskod: {response.StatusCode} info: {errorDetails}");
            }

        }

        //Hjälpmetod for GET requests (DRY)
        private async Task<HttpResponseMessage> SendGetRequestAsync(string url, string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            return response;
        }

    }
}
