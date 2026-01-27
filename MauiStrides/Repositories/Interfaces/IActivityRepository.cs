using MauiStrides.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MauiStrides.Interfaces
{
    public interface IActivityRepository
    {
        Task<List<Activity>> GetActivitiesAsync();

        Task<List<Activity>> GetActivitiesAsync(int page, int perPage);

        Task<Activity> GetActivityDetailsAsync(long activityId);
       
     


    }
}
