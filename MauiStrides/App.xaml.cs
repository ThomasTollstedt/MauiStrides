using Microsoft.Extensions.DependencyInjection;
using MauiStrides.Services;
using MauiStrides.Views;

namespace MauiStrides
{
    public partial class App : Application
    {
        private readonly ITokenService _tokenService;

        public App(ITokenService tokenService)
        {
            InitializeComponent();
            _tokenService = tokenService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var mainPage = CheckAuthenticationStatus();
            return new Window(mainPage);
        }

        private Page CheckAuthenticationStatus()
        {
            // Check if user has stored tokens
            var hasTokens = Task.Run(async () => await _tokenService.HasStoredTokensAsync()).Result;

            if (hasTokens)
            {
                return new AppShell();
            }
            else
            {
                var loginPage = IPlatformApplication.Current.Services.GetService<LoginPage>();
                return new NavigationPage(loginPage);
            }
        }
    }
}