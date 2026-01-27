using MauiStrides.ViewModels;

namespace MauiStrides.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _viewModel;

    public ProfilePage(ProfileViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel; // Sätter BindingContext till ViewModel
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadAthleteProfile();
        await _viewModel.LoadAllMonthlyStats();
        //await _viewModel.LoadCurrentMonthActivityCounter();
        //await _viewModel.LoadCurrentMonthTotalDuration();
        //await _viewModel.LoadCurrentMonthTotalDistance();
        //await _viewModel.LoadCurrentMonthActivityDetailCounter();
    }
}