using MauiStrides.ViewModels;

namespace MauiStrides.Views;

public partial class ActivityDetailsPage : ContentPage
{
	public ActivityDetailsPage(ActivityDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel; // Koppla ViewModel till sidan
    }
}