using MauiStrides.Services; // För vår Decoder
using MauiStrides.ViewModels;
using Microsoft.Maui.Controls.Maps; // För Polyline
using Microsoft.Maui.Maps; // För MapSpan

namespace MauiStrides.Views;

public partial class ActivityDetailsPage : ContentPage
{

    public ActivityDetailsPage(ActivityDetailsViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel; // Koppla ViewModel till sidan

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        
    }

   
}