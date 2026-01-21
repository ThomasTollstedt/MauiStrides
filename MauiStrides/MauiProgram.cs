using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MauiStrides.Client;
using MauiStrides.Services;
using MauiStrides.ViewModels;
using System.Reflection;

namespace MauiStrides
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
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
            builder.Services.AddHttpClient<StravaApiClient>();
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
            builder.Services.AddSingleton<IStravaService, StravaService>();
            
            // Register ViewModels and Pages
            builder.Services.AddTransient<ActivitiesViewModel>();
            builder.Services.AddTransient<ActivitiesPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
