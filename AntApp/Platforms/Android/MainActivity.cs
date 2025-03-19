using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Views;

namespace AntApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnStart()
    {
        // Prevent screen capture and preview in recent apps
        Window?.SetFlags(WindowManagerFlags.Secure, WindowManagerFlags.Secure);

        base.OnStart();
    }

    protected override void OnPause()
    {
        // Push overlay - 
        App.Current.MainPage.Navigation.PushAsync(new OverlayPage());

        base.OnPause();
    }

    protected override void OnResume()
    {
        // Pop overlay
        App.Current.MainPage.Navigation.PopAsync();

        base.OnResume();
    }
}

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter([Intent.ActionView],
              Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
              DataScheme = CALLBACK_SCHEME)]
public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
{
    const string CALLBACK_SCHEME = "ant";

}