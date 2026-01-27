using MauiStrides.Services;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using System.Diagnostics;
using static Microsoft.Maui.LifecycleEvents.WindowsLifecycle;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MauiStrides.WinUI
{
  
    public partial class App : MauiWinUIApplication
    {
        

      
        public App()
        {
            this.InitializeComponent();
           

            // 1. Vi försöker registrera oss som "Huvud-appen" med ett unikt nyckel-namn.
            var mainInstance = AppInstance.FindOrRegisterForKey("MauiStridesAppKey");

            // 2. Om vi INTE är den nuvarande huvud-appen (dvs om en annan redan körs)...
            if (!mainInstance.IsCurrent)
            {
                // ...då hämtar vi datan som startade oss (länken från Strava)
                var activationArgs = AppInstance.GetCurrent().GetActivatedEventArgs();

                // ...och skickar över den till den GAMLA appen (Redirect).
                // Vi kör detta i en Task för att inte blockera konstruktorn.
                Task.Run(async () =>
                {
                    await mainInstance.RedirectActivationToAsync(activationArgs);
                    // När vi skickat bollen, begår vi harakiri (dödar den nya processen).
                    Process.GetCurrentProcess().Kill();
                });

                // Avbryt resten av uppstarten för detta fönster
                return;
            }
            else
            {
                
                // Vi ÄR huvud-instansen. Vi måste börja lyssna på signaler från andra instanser!
                mainInstance.Activated += OnActivated;
            }
        }
        private void OnActivated(object sender, Microsoft.Windows.AppLifecycle.AppActivationArguments args)
        {
            // Kolla att det faktiskt är ett protokoll (länk)
            if (args.Kind == Microsoft.Windows.AppLifecycle.ExtendedActivationKind.Protocol)
            {
                var data = args.Data as Windows.ApplicationModel.Activation.IProtocolActivatedEventArgs;
                if (data != null)
                {
                    var uri = data.Uri.AbsoluteUri;
                    System.Diagnostics.Debug.WriteLine($"📢 [App.xaml.cs] Mottog URL: {uri}");

                    // Vi måste hoppa in på MAUI-tråden för att prata med våra Services
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        // Hämta StravaService via den globala containern
                        var stravaAuthService = IPlatformApplication.Current.Services.GetService<IStravaAuthService>();

                        // Kör din metod!
                        stravaAuthService?.HandleAuthCallbackAsync(uri);
                    });
                }
            }
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }

}
