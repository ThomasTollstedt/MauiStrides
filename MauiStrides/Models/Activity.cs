using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MauiStrides.Models
{
    public class Activity // Övar på Clean architecture därmed inget Arv från DTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public decimal Distance { get; set; }
        public int ElapsedTime { get; set; } //Seconds
        public int MovingTime { get; set; } //Seconds
        public string Type { get; set; } = "";
        public double? AverageWatts { get; set; }
        public DateTime StartDate { get; set; }
        public decimal AverageSpeed { get; set; } // m/s 
        public decimal TotalElevationGain { get; set; } // meters
        public decimal MaxSpeed { get; set; } // m/s
        public double? AverageHeartrate { get; set; }
        public double? AverageCadence { get; set; }
        public decimal? KiloJoules { get; set; }



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
                    "NordicSki" => "⛷️",
                    "BackcountrySki" => "⛷️",
                    _ => "🤸‍"
                };
            }
        }

        public string DistanceKm => $"{Distance / 1000.0m:F2} km";

        public string ElevationDisplay => $"{TotalElevationGain:F0} m";

        public string AverageSpeedDisplay => $"{AverageSpeed * 3.6m:F1} km/h";

        public string MaxSpeedDisplay => $"{MaxSpeed * 3.6m:F1} km/h";

        public string kJDisplay => KiloJoules.HasValue ? $"{KiloJoules:F0} kJ" : "-";

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

