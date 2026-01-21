using System.Text.Json.Serialization;

namespace MauiStrides.Models
{
    /// <summary>
    /// Response from Strava OAuth token endpoint
    /// </summary>
    public class StravaTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_at")]
        public long ExpiresAt { get; set; } // Unix timestamp

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; } // Seconds until expiration

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Checks if the access token is expired or will expire within 5 minutes
        /// </summary>
        public bool IsExpired()
        {
            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(ExpiresAt);
            return DateTimeOffset.UtcNow.AddMinutes(5) >= expirationTime;
        }
    }
}