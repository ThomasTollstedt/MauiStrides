using MauiStrides.Client;
using MauiStrides.Interfaces;
using MauiStrides.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly StravaApiClient _stravaApiClient;

        public ActivityRepository(StravaApiClient stravaApiClient)
        {
            _stravaApiClient = stravaApiClient;
        }

        public async Task<List<Activity>> GetActivitiesAsync()
        {
            var activitiesDto = await _stravaApiClient.GetActivitiesAsync(); // Får en lista av ActivityDTO

            var activities = activitiesDto.Select( dto => MapToDomain(dto)).ToList();
            
            return activities;
        }




        public async Task<List<Activity>> GetActivitiesAsync(int page, int perPage)
        {
            var activitiesDto = await _stravaApiClient.GetActivitiesAsync(page, perPage);

            var activities = activitiesDto.Select(dto => MapToDomain(dto)).ToList();

            return activities;
        }

        public async Task<Activity> GetActivityDetailsAsync(long activityId)
        {
            var activityDetail = await _stravaApiClient.GetActivityDetailsAsync(activityId);
            var activity = MapToDomain(activityDetail);

            return activity;
        }

    
        private Activity MapToDomain(ActivityDTO dto)
        {
            return new Activity
            {
                Name = dto.Name,
                MovingTime = dto.MovingTime,
                Distance = dto.Distance,
                Type = dto.Type,
                StartDate = dto.StartDate,
                ElapsedTime = dto.ElapsedTime,
                AverageWatts = dto.AverageWatts,
                Id = dto.Id,
                AverageSpeed = dto.AverageSpeed,
                TotalElevationGain = dto.TotalElevationGain,
                MaxSpeed = dto.MaxSpeed,
                AverageHeartrate = dto.AverageHeartrate,
                AverageCadence = dto.AverageCadence,
                KiloJoules = dto.KiloJoules
            };


        }
    }
}
