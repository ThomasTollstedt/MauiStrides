using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MauiStrides.Models
{
    public class AthleteProfileDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastname")]
        public string LastName { get; set; }

        [JsonPropertyName("bio")]
        public string Bio { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        [JsonPropertyName("weight")]
        public decimal Weight { get; set; }

        [JsonPropertyName("profile_medium")]
        public string ProfileMedium { get; set; } 

        [JsonPropertyName("profile")]
        public string Profile { get; set; }

        


    }
}
