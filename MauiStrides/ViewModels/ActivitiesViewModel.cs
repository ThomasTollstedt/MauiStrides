using CommunityToolkit.Mvvm.ComponentModel;
using MauiStrides.Models;
using MauiStrides.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MauiStrides.ViewModels
{
    public partial class ActivitiesViewModel : ObservableObject
    {
        private readonly IStravaService _stravaService;

        public ActivitiesViewModel(IStravaService stravaService)
        {
            _stravaService = stravaService;
        }

        public ObservableCollection<Activity> Activities { get; } = new();

        [ObservableProperty]
        private AthleteProfile? currentAthleteProfile;



        public async Task LoadActivitiesAsync(string accessToken, string? type = null)
        {
            Activities.Clear(); //Rensar listan innan ny data laddas in
            var allActivities = await _stravaService.GetAllActivitiesAsync(accessToken, type);
            foreach (var item in allActivities)
            {
                Activities.Add(item);
            }

        }

        public async Task LoadAthleteProfile(string accessToken)
        {
            var profile = await _stravaService.GetAthleteProfileAsync(accessToken);
            CurrentAthleteProfile = profile;

        }
    }
}
