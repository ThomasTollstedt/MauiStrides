using MauiStrides.Client;
using MauiStrides.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.Services
{
    public class StravaService : IStravaService
    {
        private readonly StravaApiClient _apiClient;
        public StravaService(StravaApiClient apiClient) => _apiClient = apiClient;
        
        public async Task<Activity> GetActivityDetailsAsync(string accessToken, long activityId)
        {
            var selectedActivity = await _apiClient.GetActivityDetailsAsync(accessToken, activityId);
            return selectedActivity;
        }

        public async Task<List<Activity>> GetAllActivitiesAsync(string accessToken, string? type = null)
        {
            
            var allActivities = await _apiClient.GetActivitiesAsync(accessToken);
            var filterActivity = allActivities
                .Where(a => a.Type.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();
                
            return filterActivity;
        }

        public async Task<AthleteProfile> GetAthleteProfileAsync(string accessToken)
        {
            var profile = await _apiClient.GetAthleteProfileAsync(accessToken);
            return profile;
        }
    }
}
