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

        // Date Range Properties
        [ObservableProperty]
        private DateTime selectedStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        [ObservableProperty]
        private DateTime selectedEndDate = DateTime.Now;

        // Sport Filter Checkboxes
        [ObservableProperty]
        private bool isRunSelected = true;

        [ObservableProperty]
        private bool isRideSelected = true;

        [ObservableProperty]
        private bool isVirtualRideSelected = true;

        [ObservableProperty]
        private bool isNordicSkiSelected = true;

        // Filtered Stats Results
        [ObservableProperty]
        private int filteredActivityCount;

        [ObservableProperty]
        private decimal filteredTotalDuration;

        [ObservableProperty]
        private decimal filteredTotalDistance;

        // Auto-refresh when date range or filters change
        partial void OnSelectedStartDateChanged(DateTime value) => _ = LoadFilteredStats();
        partial void OnSelectedEndDateChanged(DateTime value) => _ = LoadFilteredStats();
        partial void OnIsRunSelectedChanged(bool value) => _ = LoadFilteredStats();
        partial void OnIsRideSelectedChanged(bool value) => _ = LoadFilteredStats();
        partial void OnIsVirtualRideSelectedChanged(bool value) => _ = LoadFilteredStats();
        partial void OnIsNordicSkiSelectedChanged(bool value) => _ = LoadFilteredStats();

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

        // New: Load filtered stats based on date range and selected sports
        public async Task LoadFilteredStats()
        {
            var activities = await GetActivitiesInDateRangeAsync(SelectedStartDate, SelectedEndDate);

            // Build list of selected sport types
            var selectedSports = new List<string>();
            if (IsRunSelected) selectedSports.Add("Run");
            if (IsRideSelected) selectedSports.Add("Ride");
            if (IsVirtualRideSelected) selectedSports.Add("VirtualRide");
            if (IsNordicSkiSelected)
            {
                selectedSports.Add("NordicSki");
                selectedSports.Add("BackcountrySki");
            }

            // Filter activities by selected sports
            var filteredActivities = activities
                .Where(a => selectedSports.Contains(a.Type))
                .ToList();

            // Calculate totals
            FilteredActivityCount = filteredActivities.Count;
            FilteredTotalDuration = filteredActivities.Sum(a => a.MovingTime) / 3600.0m;
            FilteredTotalDistance = filteredActivities.Sum(a => a.Distance) / 1000.0m;
        }

        // Refactored: Get activities in date range (DRY principle)
        private async Task<List<Activity>> GetActivitiesInDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var activities = await _activityService.GetAllActivitiesAsync();

            return activities
                .Where(a => a.StartDate.Date >= startDate.Date && a.StartDate.Date <= endDate.Date)
                .ToList();
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
        // Existing method (kept for backward compatibility)
        private async Task<List<Activity>> GetCurrentMonthActivitiesAsync()
        {
            var now = DateTime.Now;
            return await GetActivitiesInDateRangeAsync(
                new DateTime(now.Year, now.Month, 1),
                now
            );
        }

        public async Task LoadAthleteProfile()
        {
            var profile = await _athleteService.GetAthleteProfileAsync();
            CurrentAthleteProfile = profile;
        }

        [RelayCommand]
        async Task ResetToCurrentMonth()
        {
            var now = DateTime.Now;
            SelectedStartDate = new DateTime(now.Year, now.Month, 1);
            SelectedEndDate = now;

            // Reset all filters to checked
            IsRunSelected = true;
            IsRideSelected = true;
            IsVirtualRideSelected = true;
            IsNordicSkiSelected = true;
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
