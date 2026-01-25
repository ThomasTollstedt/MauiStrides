using Microsoft.Extensions.DependencyInjection;
using MauiStrides.Services;
using MauiStrides.Views;

namespace MauiStrides
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}