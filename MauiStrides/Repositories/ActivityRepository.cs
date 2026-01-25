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

        public async Task<AthleteProfile> GetAthleteProfileAsync()
        {
            var profileDetail = await _stravaApiClient.GetAthleteProfileAsync();
            var profile = MapToDomainProfile(profileDetail);

            return profile;
        }

        private AthleteProfile MapToDomainProfile(AthleteProfileDTO dto)
        {
            return new AthleteProfile
            { 
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Id = dto.Id,
            Bio = dto.Bio,
            City = dto.City,
            State = dto.State,
            Country = dto.Country,
            Sex = dto.Sex,
            Weight = dto.Weight,
            ProfileMedium = dto.ProfileMedium,
            Profile = dto.Profile
            };
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
