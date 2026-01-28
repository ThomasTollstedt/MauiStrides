using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiStrides.Models;
using MauiStrides.Services;
using MauiStrides.Views;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;

namespace MauiStrides.ViewModels
{
    public partial class ActivitiesViewModel : ViewModelBase
    {
        private readonly IActivityService _activityService;

        private int _currentPage = 1;
        private const int _pageSize = 20;
        private bool _isLastPageReached = false;

       
        // Master list: All activities fetched from API
        private readonly List<Activity> _allFetchedActivities = new();

        // Filtered list: What the UI displays (Observable)
        public ObservableCollection<Activity> Activities { get; } = new();

      

        [ObservableProperty]
        private string searchText = "";
        partial void OnSearchTextChanged(string value) => FilterActivities();

        [ObservableProperty]
        private string selectedFilter = "All";
        partial void OnSelectedFilterChanged(string value) => FilterActivities();

        [ObservableProperty]
        private bool isFooterLoading;

      
        public ActivitiesViewModel(IActivityService activityService)
        {
            _activityService = activityService;
            Title = "Aktiviteter";
        }

    
        public async Task LoadActivitiesAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            _currentPage = 1;
            _isLastPageReached = false;

            // Cleara båda listorna för ny inläsning
            _allFetchedActivities.Clear();
            Activities.Clear();

            try
            {
                // Fetch first page from API
                var activities = await _activityService.GetActivitiesAsync(_currentPage, _pageSize);

                // Checka om detta är sista sidan genom antal aktiviteter fetchade.
                if (activities.Count < _pageSize)
                {
                    _isLastPageReached = true;
                }

                // Till master-listan.
                _allFetchedActivities.AddRange(activities);

                // Hjälpmetod filtrering och uppdatering av ObservableCollection
                FilterActivities();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading activities: {ex.Message}");
                
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Inifinite scroll vid 5 items från botten
        [RelayCommand]
        public async Task LoadMore()
        {
            // Stoppar laddning om redan laddar, sista sidan nådd eller upptagen
            if (IsFooterLoading || _isLastPageReached || IsBusy) return;

            IsFooterLoading = true;

            try
            {
                // Increment page counter
                _currentPage++;

                // Fetch next page from API
                var newActivities = await _activityService.GetActivitiesAsync(_currentPage, _pageSize);

                // Check if this is the last page
                if (newActivities.Count < _pageSize)
                {
                    _isLastPageReached = true;
                }

                // Add new activities to master list
                _allFetchedActivities.AddRange(newActivities);

                // Re-apply filter to include new items if they match current filter
                FilterActivities();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading more activities: {ex.Message}");
                // Revert page counter if load failed
                _currentPage--;
            }
            finally
            {
                IsFooterLoading = false;
            }
        }

        private void FilterActivities()
        {
            // Start with all fetched activities from master list
            var filtered = _allFetchedActivities.AsEnumerable();

            // Filter by search text (name)
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(a =>
                    a.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by activity type (if not "All")
            if (SelectedFilter != "All")
            {
                filtered = filtered.Where(a =>
                    a.Type.Equals(SelectedFilter, StringComparison.OrdinalIgnoreCase));
            }

            // Update the observable collection (what UI sees)
            Activities.Clear();
            foreach (var activity in filtered)
            {
                Activities.Add(activity);
            }
        }

       
        [RelayCommand]
        void ApplyFilter(string filterType)
        {
            // Command för att sätta vald filtertyp från UI och meddela förändring.
            SelectedFilter = filterType;
            OnPropertyChanged(nameof(SelectedFilter));
        }

       
        [RelayCommand]
        [SupportedOSPlatform("windows10.0.17763.0")]
        async Task GoToDetails(Activity activity)
        {
            if (activity == null) return;

            await Shell.Current.GoToAsync(nameof(ActivityDetailsPage), new Dictionary<string, object>
            {
                { "Activity", activity }
            });
        }

    }
}
