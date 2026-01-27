using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MauiStrides.Client;
using MauiStrides.Services;
using MauiStrides.ViewModels;
using System.Reflection;
using MauiStrides.Views;
using Microsoft.Maui.LifecycleEvents;
using Microcharts.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;
using MauiStrides.Interfaces;
using MauiStrides.Repositories;
using MauiStrides.Services.Interfaces;
using MauiStrides.Repositories.Interfaces;
#if WINDOWS
using Microsoft.Windows.AppLifecycle;
using Windows.ApplicationModel.Activation;
#endif

namespace MauiStrides
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMicrocharts()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeFluentIcons.ttf", "FluentIcons");
                })
                .ConfigureLifecycleEvents(events =>
                {
#if WINDOWS
                events.AddWindows(windows => windows
                    .OnWindowCreated(window =>
        {
           // Simple mobile-friendly window for development
            window.ExtendsContentIntoTitleBar = false;
            
            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            
            // iPhone 14 Pro size for testing
            appWindow.Resize(new Windows.Graphics.SizeInt32(393, 852));
            
        }) 
                
                
                
                .OnActivated((window, args) =>
                    {
                        // Vi hämtar aktiverings-datan från den globala instansen istället för 'args'
                        // Detta undviker krockar och CS-fel.
                        var appInstance = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent();
                        if (appInstance == null) return;

                        var activationArgs = appInstance.GetActivatedEventArgs();

                        // Kolla om appen väcktes av ett protokoll (mauistrides://)
                        if (activationArgs.Kind == Microsoft.Windows.AppLifecycle.ExtendedActivationKind.Protocol)
                        {
                            // Konvertera datan till rätt typ
                            var data = activationArgs.Data as Windows.ApplicationModel.Activation.IProtocolActivatedEventArgs;
                            
                            if (data != null)
                            {
                                var uri = data.Uri.AbsoluteUri;
                                System.Diagnostics.Debug.WriteLine($"📢 [Windows] Protocol URL: {uri}");

                                // Hämta StravaService och skicka in URL:en
                                // Vi måste köra detta på MainThread för att vara säkra
                                MainThread.BeginInvokeOnMainThread(() => 
                                {
                                    var stravaAuthService = IPlatformApplication.Current.Services.GetService<MauiStrides.Services.StravaAuthService>();
                                    stravaAuthService?.HandleAuthCallbackAsync(uri);
                                });
                            }
                        }
                    }));
#endif
                });

            // ✅ LOAD CONFIGURATION FROM appsettings.json
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("MauiStrides.appsettings.json");
            
            if (stream == null)
            {
                throw new FileNotFoundException("appsettings.json not found. Please create it from appsettings.json.template");
            }

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            // Register configuration
            builder.Services.AddSingleton<IConfiguration>(config);

            // Register HttpClients
            builder.Services.AddHttpClient<StravaApiClient>(client =>
                { 
                
                client.BaseAddress = new Uri("https://www.strava.com/api/v3/");
                }

                );
            builder.Services.AddHttpClient<TokenService>();
            
            // Register Strava Configuration from appsettings.json
            builder.Services.AddSingleton<StravaConfiguration>(sp => 
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new StravaConfiguration
                {
                    ClientId = configuration["Strava:ClientId"] 
                        ?? throw new InvalidOperationException("Strava:ClientId not configured in appsettings.json"),
                    ClientSecret = configuration["Strava:ClientSecret"] 
                        ?? throw new InvalidOperationException("Strava:ClientSecret not configured in appsettings.json")
                };
            });
            
            // Register services
            builder.Services.AddSingleton<ITokenService, TokenService>();
            builder.Services.AddSingleton<IActivityService, ActivityService>();
            builder.Services.AddSingleton<IStravaAuthService, StravaAuthService>();
            builder.Services.AddSingleton<IActivityRepository, ActivityRepository>();
            builder.Services.AddSingleton<IAthleteService, AthleteService>();
            builder.Services.AddSingleton<IAthleteRepository, AthleteRepository>();
            
            // Register ViewModels and Pages
            builder.Services.AddTransient<ActivitiesViewModel>();
            builder.Services.AddTransient<ActivitiesPage>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ActivityDetailsViewModel>();
            builder.Services.AddTransient<ActivityDetailsPage>();
            builder.Services.AddTransient<SummaryViewModel>();
            builder.Services.AddTransient<SummaryPage>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<ProfilePage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
