
using MauiStrides.Interfaces;
using MauiStrides.Models;

namespace MauiStrides.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
      
        public ActivityService(IActivityRepository activityRepository) =>  _activityRepository = activityRepository;
           
        

        public async Task<List<Activity>> GetActivitiesAsync(int page, int perPage)
        {
            return await _activityRepository.GetActivitiesAsync(page, perPage);
        }


        public async Task<List<Activity>> GetAllActivitiesAsync(string? type = null)
        {
            var allActivities = await _activityRepository.GetActivitiesAsync();

            if (!string.IsNullOrWhiteSpace(type))
            {
                return allActivities
                    .Where(a => a.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return allActivities;
        }

        public async Task<Activity> GetActivityDetailsAsync(long activityId)
        {
            return await _activityRepository.GetActivityDetailsAsync(activityId);
        }
    }
}

