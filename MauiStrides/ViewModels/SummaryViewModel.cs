using CommunityToolkit.Mvvm.ComponentModel;
using MauiStrides.Services;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MauiStrides.Models;

namespace MauiStrides.ViewModels
{
    public partial class SummaryViewModel : ViewModelBase
    {
        private readonly IActivityService _activityService;

        [ObservableProperty]
        private Chart distanceChart = new DonutChart
        {
            Entries = Array.Empty<ChartEntry>(),
            BackgroundColor = SKColors.White
        };

        [ObservableProperty]
        private Chart activityCountChart = new LineChart
        {
            Entries = Array.Empty<ChartEntry>(),
            BackgroundColor = SKColors.White
        };

        [ObservableProperty]
        private bool hasError;

        [ObservableProperty]
        private string errorMessage = "";

        public SummaryViewModel(IActivityService activityService)
        {
            _activityService = activityService;
            Title = "Statistics";
        }

        public async Task LoadAllChartsAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            HasError = false;
            ErrorMessage = "";

            try
            {
                System.Diagnostics.Debug.WriteLine("📊 [SummaryViewModel] Starting chart load...");

                var activities = await _activityService.GetAllActivitiesAsync();

                if (activities == null)
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ [SummaryViewModel] Activities is NULL");
                    HasError = true;
                    ErrorMessage = "No data available";
                    return;
                }

                // Create snapshot once
                var safeList = activities.ToList();

                // ✅ Use safeList from here on
                if (!safeList.Any())
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ [SummaryViewModel] No activities found");
                    HasError = true;
                    ErrorMessage = "No activities found";
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"✅ [SummaryViewModel] Loaded {safeList.Count} activities");

                // Pass safeList (already a List)
                await LoadDistanceChartAsync(safeList);
                System.Diagnostics.Debug.WriteLine("✅ Distance chart completed");

                //await LoadActivityCountChartAsync(safeList);
                //System.Diagnostics.Debug.WriteLine("✅ Activity count chart completed");
                

                //await Task.WhenAll(
                //    LoadDistanceChartAsync(safeList),
                //    LoadActivityCountChartAsync(safeList)
                //);

                                System.Diagnostics.Debug.WriteLine("✅ [SummaryViewModel] All charts loaded successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ [SummaryViewModel] ERROR: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                HasError = true;
                ErrorMessage = $"Failed to load statistics: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Utkommentad för att undvika krascher just nu
        //private async Task LoadActivityCountChartAsync(IEnumerable<Activity> activities)
        //{
        //    try
        //    {
        //        System.Diagnostics.Debug.WriteLine("📈 [LoadActivityCount] Starting...");

        //        var chart = await Task.Run(() =>
        //        {

        //            //Omgjord då krasch pga tomma datum i vissa aktiviteter (tror jag) då GroupBy StartDate.Year, StartDate.Month används direkt.
        //            var monthlyData = activities
        //                .Where(a => a.StartDate != DateTime.MinValue) // Säkra upp mot "tomma" structs
        //                .GroupBy(a => new { a.StartDate.Year, a.StartDate.Month })
        //                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
        //                .Select(g => new
        //                {
        //                    MonthLabel = $"{g.Key.Year}-{g.Key.Month:D2}",
        //                    ActivityCount = g.Count()

        //                })
        //                .ToList();

        //            System.Diagnostics.Debug.WriteLine($"📈 [LoadActivityCount] Grouped into {monthlyData.Count} months");

        //            if (!monthlyData.Any())
        //            {
        //                System.Diagnostics.Debug.WriteLine("⚠️ [LoadActivityCount] No monthly data");
        //                return new LineChart
        //                {
        //                    Entries = Array.Empty<ChartEntry>(),
        //                    BackgroundColor = SKColors.White
        //                };
        //            }

        //            var entries = monthlyData.Select(item => new ChartEntry(item.ActivityCount)
        //            {
        //                Label = item.MonthLabel,
        //                ValueLabel = item.ActivityCount.ToString(),
        //                Color = SKColors.CornflowerBlue,
        //                TextColor = SKColors.Black
        //            }).ToList();

        //            return new LineChart
        //            {
        //                Entries = entries,
        //                LineMode = LineMode.Straight,
        //                LineSize = 4,
        //                PointMode = PointMode.Circle,
        //                PointSize = 8,
        //                BackgroundColor = SKColors.White
        //            };
        //        }); 

        //        await MainThread.InvokeOnMainThreadAsync(() =>
        //        {
        //            ActivityCountChart = chart;
        //            System.Diagnostics.Debug.WriteLine("✅ [LoadActivityCount] Chart assigned to UI");
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"❌ [LoadActivityCount] ERROR: {ex.Message}");
        //        throw;
        //    }
        //}

        private async Task LoadDistanceChartAsync(IEnumerable<Activity> activities)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("🍩 [LoadDistance] Starting...");

                // ✅ Remove redundant .ToList() - activities is already a List
                var chart = await Task.Run(() =>
                {
                    var groupedData = activities
                        .Where(a => a.Type != null &&
                                   (a.Type == "Run" || a.Type == "Ride" || a.Type == "VirtualRide"))
                        .GroupBy(a => a.Type)
                        .Select(g => new
                        {
                            Type = g.Key,
                            TotalDistanceKm = g.Sum(a => (double)a.Distance) / 1000.0
                        })
                        .ToList();

                    System.Diagnostics.Debug.WriteLine($"🍩 [LoadDistance] Grouped into {groupedData.Count} types");

                    if (!groupedData.Any())
                    {
                        System.Diagnostics.Debug.WriteLine("⚠️ [LoadDistance] No grouped data");
                        return new DonutChart
                        {
                            Entries = Array.Empty<ChartEntry>(),
                            BackgroundColor = SKColors.White
                        };
                    }

                    var entries = groupedData.Select(item => new ChartEntry((float)item.TotalDistanceKm)
                    {
                        Label = item.Type,
                        ValueLabel = $"{item.TotalDistanceKm:F1} km",
                        Color = GetColorForType(item.Type),
                        TextColor = SKColors.Black
                    }).ToList();

                    return new DonutChart
                    {
                        Entries = entries,
                        LabelTextSize = 40,
                        HoleRadius = 0.5f,
                        BackgroundColor = SKColors.White
                    };
                });

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    DistanceChart = chart;
                    System.Diagnostics.Debug.WriteLine("✅ [LoadDistance] Chart assigned to UI");
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ [LoadDistance] ERROR: {ex.Message}");
                throw;
            }
        }

        private SKColor GetColorForType(string type)
        {
            return type switch
            {
                "Run" => SKColors.Crimson,
                "Ride" => SKColors.LightGreen,
                "VirtualRide" => SKColors.Orange,
                "Ski" => SKColors.LightSkyBlue,
                _ => SKColors.LightGray
            };
        }
    }
}
