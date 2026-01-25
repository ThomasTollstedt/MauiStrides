namespace MauiStrides.Services
{
    /// <summary>
    /// Configuration for Strava API credentials
    /// </summary>
    public class StravaConfiguration
    {
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}