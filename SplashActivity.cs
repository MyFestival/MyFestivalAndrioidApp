using System.ServiceModel;
using System.Threading;
using Android.App;
using Android.OS;

namespace MyFestivalApp
{
    [Activity(Theme = "@style/Theme.Splash", NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			Thread.Sleep(500); // Simulate a long loading process on app startup.
            StartActivity(typeof(Activity1));
        }
    }
}
