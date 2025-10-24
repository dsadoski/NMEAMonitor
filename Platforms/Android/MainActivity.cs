using Android.App;
using Android.Content.PM;
using Android.OS;

namespace NMEAMon
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {

        public MainActivity()
        {
            if (Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().Result != PermissionStatus.Granted)
            {
                //var status =  Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
        }
    }
}
