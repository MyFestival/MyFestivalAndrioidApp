using System.ServiceModel;
using System.Threading;
using Android.App;
using Android.OS;

namespace MyFestivalApp
{
	[Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			Thread.Sleep(600);
            StartActivity(typeof(Activity1));
        }
    }
}
