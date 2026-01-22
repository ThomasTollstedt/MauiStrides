using MauiStrides.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.Services
{
    public interface IStravaService
    {
        Task<List<Activity>> GetAllActivitiesAsync(string? type = null);
        Task<AthleteProfile> GetAthleteProfileAsync();
        Task<Activity> GetActivityDetailsAsync(long activityId);
        Task LoginServiceAsync();
        Task HandleAuthCallbackAsync(string uriString);
    }
}
