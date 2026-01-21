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



        [JsonPropertyName("distance")]

        public decimal Distance { get; set; }



        [JsonPropertyName("elapsed_time")]

        public int ElapsedTime { get; set; } //Seconds



        [JsonPropertyName("type")]

        public string Type { get; set; } = "";

        [JsonPropertyName("average_watts")]
        public double AverageWatts { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("average_speed")]
        public decimal AverageSpeed { get; set; } // m/s 

    }
}
