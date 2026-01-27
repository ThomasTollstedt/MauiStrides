using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiStrides.Models;
using MauiStrides.Services;
using MauiStrides.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text;

namespace MauiStrides.ViewModels
{
    public partial class ProfileViewModel : ViewModelBase
    {
        private readonly IAthleteService _athleteService;
        private readonly IActivityService _activityService;
        private readonly IStravaAuthService _stravaAuthService;

        public ProfileViewModel(IAthleteService athleteService, IActivityService activityService, IStravaAuthService stravaAuthService)
        {
            _athleteService = athleteService;
            _activityService = activityService;
            _stravaAuthService = stravaAuthService;
        }

        [ObservableProperty]
        private AthleteProfile? currentAthleteProfile;

        [ObservableProperty]
        private int currentMonthActivityCount;

        [ObservableProperty]
        private decimal currentMonthTotalDuration;

        [ObservableProperty]
        private decimal currentMonthTotalDistance;

        [ObservableProperty]
        private int currentMonthRunCount;

        [ObservableProperty]
        private int currentMonthRideCount;

        [ObservableProperty]
        private int currentMonthVirtualRideCount;


        // Load all monthly stats för att spara API-calls
        public async Task LoadAllMonthlyStats()
        {
            var currentMonthActivities = await GetCurrentMonthActivitiesAsync();

            // Overall stats
            CurrentMonthActivityCount = currentMonthActivities.Count;
            CurrentMonthTotalDuration = currentMonthActivities.Sum(a => a.MovingTime) / 3600.0m;
            CurrentMonthTotalDistance = currentMonthActivities.Sum(a => a.Distance) / 1000.0m;

            // By type
            CurrentMonthRunCount = currentMonthActivities.Count(a => a.Type == "Run");
            CurrentMonthRideCount = currentMonthActivities.Count(a => a.Type == "Ride");
            CurrentMonthVirtualRideCount = currentMonthActivities.Count(a => a.Type == "VirtualRide");



        }
        //public async Task LoadCurrentMonthActivityDetailCounter()
        //{
        //    var currentMonthActivities = await GetCurrentMonthActivitiesAsync();
               
        //    CurrentMonthRunCount = currentMonthActivities.Count(a => a.Type == "Run");
        //    CurrentMonthRideCount = currentMonthActivities.Count(a => a.Type == "Ride");
        //    CurrentMonthVirtualRideCount = currentMonthActivities.Count(a => a.Type == "VirtualRide");

        //}
        //public async Task LoadCurrentMonthActivityCounter()
        //{
        //    var currentMonthActivities = await GetCurrentMonthActivitiesAsync();
        //    CurrentMonthActivityCount = currentMonthActivities
        //    .Count();

        //}

        //public async Task LoadCurrentMonthTotalDuration()
        //{
        //    var currentMonthActivities = await GetCurrentMonthActivitiesAsync();
        //    var totalSeconds = currentMonthActivities.Sum(a => a.MovingTime);
        //    //convert 
        //    CurrentMonthTotalDuration = totalSeconds / 3600.0m; // Convert seconds to hours

        //}

        //public async Task LoadCurrentMonthTotalDistance()
        //{
        //    var currentMonthActivities = await GetCurrentMonthActivitiesAsync();
        //    var totalMeters = currentMonthActivities.Sum(a => a.Distance);
        //    //convert 
        //    CurrentMonthTotalDistance = totalMeters / 1000.0m; // Convert meters to kilometers

        //}
        private async Task<List<Activity>> GetCurrentMonthActivitiesAsync()
        {
            var activities = await _activityService.GetAllActivitiesAsync();

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            return activities
                .Where(a => a.StartDate.Month == currentMonth && a.StartDate.Year == currentYear)
                .ToList();
        }

        public async Task LoadAthleteProfile()
        {
            var profile = await _athleteService.GetAthleteProfileAsync();
            CurrentAthleteProfile = profile;
        }


        [RelayCommand]
        [SupportedOSPlatform("windows10.0.17763.0")]
        async Task GoToProfile()
        {
            await Shell.Current.GoToAsync("///ProfilePage"); // Absolute routing to ProfilePage
        }

        [RelayCommand]
        async Task Logout()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Logout",
                "Are you sure you want to log out?",
                "Yes",
                "No");

            if (confirm)
            {
                await _stravaAuthService.LogoutAsync();
            }
        }

    }
}
