using MauiStrides.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.Services
{
    public interface IStravaService
    {
        Task<Activity> GetActivityDetailsAsync(string accessToken, long activityId);


        Task<List<Activity>> GetAllActivitiesAsync(string accessToken, string? type = null);


        Task<AthleteProfile> GetAthleteProfileAsync(string accessToken);
       

    }
}
