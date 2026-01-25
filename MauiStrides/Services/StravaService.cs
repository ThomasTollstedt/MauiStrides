using MauiStrides.Client;
using MauiStrides.Models;

namespace MauiStrides.Services
{
    public class StravaService : IStravaService
    {
        private readonly StravaApiClient _apiClient;
        private readonly ITokenService _tokenService;

        public StravaService(StravaApiClient apiClient, ITokenService tokenService)
        {

            _apiClient = apiClient;
            _tokenService = tokenService;
        }

        public async Task<List<Activity>> GetActivitiesAsync(int page, int perPage)
        {
            return await _apiClient.GetActivitiesAsync(page, perPage);
        }


        public async Task<List<Activity>> GetAllActivitiesAsync(string? type = null)
        {
            var allActivities = await _apiClient.GetActivitiesAsync();

            if (!string.IsNullOrWhiteSpace(type))
            {
                return allActivities
                    .Where(a => a.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return allActivities;
        }

        public async Task<AthleteProfile> GetAthleteProfileAsync()
        {
            return await _apiClient.GetAthleteProfileAsync();
        }

        public async Task<Activity> GetActivityDetailsAsync(long activityId)
        {
            return await _apiClient.GetActivityDetailsAsync(activityId);
        }

        public async Task HandleAuthCallbackAsync(string uriString)
        {
            try
            {
                var uri = new Uri(uriString);
                // Hämta koden från URL:en (mauistrides://callback?code=xyz...)
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                string code = query["code"];

                if (!string.IsNullOrEmpty(code))
                {
                    await _tokenService.LoginAsync(code);

                    // Navigate to main app
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage = new AppShell();
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Callback Error: {ex.Message}");
            }
        }

        public async Task LoginServiceAsync()
        {
            try
            {

                string callbackUrl = "mauistrides://localhost/callback";
                // Konstruera inloggnings-URL:en
                string loginUrl = "https://www.strava.com/oauth/mobile/authorize"
                    + "?client_id=196337"
                    + "&response_type=code"
                    + $"&redirect_uri={callbackUrl}"
                    + "&scope=activity:read_all"
                    + "&approval_prompt=force";


                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    // PÅ WINDOWS: Öppna bara webbläsaren och släpp taget.
                    // Vi fångar svaret i MauiProgram.cs senare.
                    await Launcher.OpenAsync(new Uri(loginUrl));
                }
                else
                {
                    // PÅ ANDROID: Använd den vanliga WebAuthenticator som vi vet funkar där
                    var result = await WebAuthenticator.Default.AuthenticateAsync(
                        new Uri(loginUrl),
                        new Uri(callbackUrl));

                    if (result?.Properties != null && result.Properties.ContainsKey("code"))
                    {
                        await _tokenService.LoginAsync(result.Properties["code"]);
                        await Shell.Current.GoToAsync("//ActivitiesPage");
                    }
                }
            }

            catch (Exception ex)
            {
                var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                if (mainPage != null)
                {
                    await mainPage.DisplayAlertAsync("Fel", $"Inloggning misslyckades: {ex.Message}", "OK");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Inloggning misslyckades: {ex.Message}");
                }
            }
        }


    }
}
