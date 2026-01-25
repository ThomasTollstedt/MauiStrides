using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MauiStrides.Models
{
    public class Activity
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

        [JsonPropertyName("calories")]
        public double? Calories { get; set; }

        // DISPLAY PROPERTIES
        public string SportIcon
        {
            get
            {
                return Type switch
                {
                    "Run" => "🏃‍",
                    "Ride" => "🚴",
                    "VirtualRide" => "🎮",
                    "Walk" => "🚶",
                    "Swim" => "🏊",
                    _ => "🤸‍"
                };
            }
        }

        public string DistanceKm => $"{Distance / 1000.0m:F2} km";

        public string ElevationDisplay => $"{TotalElevationGain:F0} m";

        public string AverageSpeedDisplay => $"{AverageSpeed * 3.6m:F1} km/h";

        public string MaxSpeedDisplay => $"{MaxSpeed * 3.6m:F1} km/h";

        public string CaloriesDisplay => Calories.HasValue ? $"{Calories:F0} kcal" : "-";

        public string HeartrateDisplay => AverageHeartrate.HasValue 
            ? $"{AverageHeartrate:F0} bpm" 
            : "-";

        public string CadenceDisplay => AverageCadence.HasValue 
            ? $"{AverageCadence:F0} rpm" 
            : "-";

        public string PowerDisplay => AverageWatts.HasValue 
            ? $"{AverageWatts:F0} W" 
            : "-";

        public string PaceDisplay
        {
            get
            {
                if (Distance <= 0 || MovingTime <= 0) return "-:-- /km";

                double totalMinutes = MovingTime / 60.0;
                double totalKm = (double)Distance / 1000.0;
                double pace = totalMinutes / totalKm;

                int paceMin = (int)pace;
                int paceSec = (int)((pace - paceMin) * 60);

                return $"{paceMin}:{paceSec:D2} /km";
            }
        }

        public string TimeDisplay => TimeSpan.FromSeconds(MovingTime).ToString(@"hh\:mm\:ss");

        public string ElapsedTimeDisplay => TimeSpan.FromSeconds(ElapsedTime).ToString(@"hh\:mm\:ss");

        public string DateDisplay => StartDate.ToString("dd MMM yyyy • HH:mm");
    }
}
