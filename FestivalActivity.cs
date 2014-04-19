using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Interop;

namespace MyFestivalApp
{
    [Activity(Label = "Festival Details", Theme = "@style/Theme.AppCompat.Light")]
    public class FestivalActivity : ActionBarActivity
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