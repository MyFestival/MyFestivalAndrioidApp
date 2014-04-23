using Android.App;
using Android.OS;

namespace MyFestivalApp
{
    [Activity(Label = "Festival Details", Theme = "@style/Theme.AppCompat.Light")]
	public class FestivalActivity : Activity
    {
		//private ShareActionProvider actionProvider;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Load the UI defined in festival.axml
			SetContentView(Resource.Layout.festival);
        }
    }
}