using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiStrides.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IStravaAuthService _stravaAuthService;

        public LoginViewModel(IStravaAuthService stravaAuthService)
        {
            _stravaAuthService = stravaAuthService;

        }

        [RelayCommand]
        public async Task Login()
        {
            try
            {
                await _stravaAuthService.LoginServiceAsync();
            }
            catch (Exception ex)
            {
                // Om något smäller, visa en ruta med felet!
                //await Application.Current.MainPage.DisplayAlertAsync("Oj!", $"Något gick fel: {ex.Message}", "OK");

                // Logga felet till konsolen också
                System.Diagnostics.Debug.WriteLine($"LOGIN ERROR: {ex}");
            }

        }
    }
}