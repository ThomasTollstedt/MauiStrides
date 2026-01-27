using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MauiStrides.Models
{
    public class ActivityDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("distance")]
        public decimal Distance { get; set; }

        [JsonPropertyName("elapsed_time")]
        public int ElapsedTime { get; set; } //Seconds

        [JsonPropertyName("moving_time")]
        public int MovingTime { get; set; } //Seconds

        [JsonPropertyName("type")]
        public string Type { get; set; } = "";

        [JsonPropertyName("average_watts")]
        public double? AverageWatts { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("average_speed")]
        public decimal AverageSpeed { get; set; } // m/s 

        [JsonPropertyName("total_elevation_gain")]
        public decimal TotalElevationGain { get; set; } // meters

        // NEW: Additional metrics from Strava
        [JsonPropertyName("max_speed")]
        public decimal MaxSpeed { get; set; } // m/s

        [JsonPropertyName("average_heartrate")]
        public double? AverageHeartrate { get; set; }

        [JsonPropertyName("average_cadence")]
        public double? AverageCadence { get; set; }

        [JsonPropertyName("kilojoules")]
        public decimal? KiloJoules { get; set; }


    }
}