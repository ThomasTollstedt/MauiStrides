using MauiStrides.ViewModels;

namespace MauiStrides.Views;

public partial class SummaryPage : ContentPage
{
    private readonly SummaryViewModel _viewModel;

    public SummaryPage(SummaryViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        
		
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAllChartsAsync(); // Ladda diagramdata när sidan visas
    }
}