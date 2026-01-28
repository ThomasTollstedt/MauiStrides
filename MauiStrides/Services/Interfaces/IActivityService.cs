using MauiStrides.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.Services
{
    public interface IActivityService
    {

        Task<List<Activity>> GetActivitiesAsync(int page, int perPage);
        Task<List<Activity>> GetAllActivitiesAsync(string? type = null);
        Task<Activity> GetActivityDetailsAsync(long activityId);
        
      
    }
}
