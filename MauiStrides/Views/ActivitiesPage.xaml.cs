
using MauiStrides.ViewModels;
using System.Runtime.CompilerServices;

namespace MauiStrides.Views;

public partial class ActivitiesPage : ContentPage
{
    private readonly ActivitiesViewModel _viewModel;
    private ProfileViewModel _profileViewModel;

    public ProfileViewModel ProfileViewModel
    {
        get => _profileViewModel;

        private set
        {
            if (_profileViewModel != value)
            {
                _profileViewModel = value;
                OnPropertyChanged();
            }
        }

    }


    public ActivitiesPage(ActivitiesViewModel viewModel, ProfileViewModel profileViewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        ProfileViewModel = profileViewModel; // Sätter mot en publik property ovan
        BindingContext = viewModel; // Ui mot kod
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await ProfileViewModel.LoadAthleteProfile();
        await _viewModel.LoadActivitiesAsync();
    }

    // ContentPage already implements INotifyPropertyChanged, but we need to make sure we can call it
    private new void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
    }
}