using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiStrides.Models;
using MauiStrides.Services;
using MauiStrides.Views;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;

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
        [RelayCommand]
        [SupportedOSPlatform("windows10.0.17763.0")]
        async Task GoToDetails(Activity activity)
        {
            if (activity == null) return;

            // Vi skickar med hela aktivitetsobjektet till nästa sida
            await Shell.Current.GoToAsync(nameof(ActivityDetailsPage), new Dictionary<string, object>
    {
        { "Activity", activity }
    });

            // Nollställ valet i listan så den inte är "vald" när man kommer tillbaka
            // (Krävs oftast lite extra kod i View för att nollställa visuellt, men detta är en bra start)
        }

        [RelayCommand]
        [SupportedOSPlatform("windows10.0.17763.0")]
        async Task GoToProfile()
        {
            // Navigera till profilsidan (som vi inte skapat än)
            await Shell.Current.GoToAsync("ProfilePage");
        }
    }
}
