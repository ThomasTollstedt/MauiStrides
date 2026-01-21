using MauiStrides.ViewModels;

namespace MauiStrides;

public partial class ActivitiesPage : ContentPage
{
    private readonly ActivitiesViewModel _viewModel;

    public ActivitiesPage(ActivitiesViewModel viewModel)
	{
        _viewModel = viewModel;
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // ? No token needed - TokenService handles it automatically
        await _viewModel.LoadActivitiesAsync();
    }
}