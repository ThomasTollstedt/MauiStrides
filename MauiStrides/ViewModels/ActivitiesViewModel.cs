using CommunityToolkit.Mvvm.ComponentModel;
using MauiStrides.Models;
using MauiStrides.Services;
using System.Collections.ObjectModel;

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

        // ✅ No more accessToken parameters!
        public async Task LoadActivitiesAsync(string? type = null)
        {
            Activities.Clear();
            var allActivities = await _stravaService.GetAllActivitiesAsync(type);
            foreach (var item in allActivities)
            {
                Activities.Add(item);
            }
        }

        public async Task LoadAthleteProfile()
        {
            var profile = await _stravaService.GetAthleteProfileAsync();
            CurrentAthleteProfile = profile;
        }
    }
}
