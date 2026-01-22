using MauiStrides.Services;
using MauiStrides.ViewModels;

namespace MauiStrides.Views;

public partial class LoginPage : ContentPage
{
    

    public LoginPage(LoginViewModel viewModel)
	{
	
		InitializeComponent();

		BindingContext = viewModel;
	}

   
}