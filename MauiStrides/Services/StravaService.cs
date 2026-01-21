using MauiStrides.Client;
using MauiStrides.Models;

namespace MauiStrides.Services
{
    public class StravaService : IStravaService
    {
        private readonly StravaApiClient _apiClient;
        
        public StravaService(StravaApiClient apiClient) => _apiClient = apiClient;
        
        // ✅ No more accessToken parameters - handled automatically!
        
        public async Task<List<Activity>> GetAllActivitiesAsync(string? type = null)
        {
            var allActivities = await _apiClient.GetActivitiesAsync();
            
            if (!string.IsNullOrWhiteSpace(type))
            {
                return allActivities
                    .Where(a => a.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            
            return allActivities;
        }

        public async Task<AthleteProfile> GetAthleteProfileAsync()
        {
            return await _apiClient.GetAthleteProfileAsync();
        }
        
        public async Task<Activity> GetActivityDetailsAsync(long activityId)
        {
            return await _apiClient.GetActivityDetailsAsync(activityId);
        }
    }
}
