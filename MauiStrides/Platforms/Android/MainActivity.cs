using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MauiStrides
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            System.Diagnostics.Debug.WriteLine("📢 [MainActivity] OnCreate körs (Appen startar/startar om)");
        }

        // 2. Vi fångar upp när Chrome skickar tillbaka oss hit
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            System.Diagnostics.Debug.WriteLine("📢 [MainActivity] OnNewIntent körs! (Vi är tillbaka)");

            // Låt oss se vad Chrome skickade med
            var data = intent?.Data;
            if (data != null)
            {
                System.Diagnostics.Debug.WriteLine($"📢 [MainActivity] Mottog URL: {data.ToString()}");

                // HÄR är det fula tricket: Vi tvingar WebAuthenticator att vakna
                Microsoft.Maui.Authentication.WebAuthenticator.Default.OnResume(intent);
            }
        }
    }
}
