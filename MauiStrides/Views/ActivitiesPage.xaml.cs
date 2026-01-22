
using MauiStrides.ViewModels;

namespace MauiStrides.Views;

public partial class ActivitiesPage : ContentPage
{
    private readonly ActivitiesViewModel _viewModel;

    public ActivitiesPage(ActivitiesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
       
        BindingContext = viewModel; // Ui mot kod
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        await _viewModel.LoadAthleteProfile();
        await _viewModel.LoadActivitiesAsync();
    }
}