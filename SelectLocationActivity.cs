using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MyFestivalApp
{
    [Activity(Label = "{0}", Theme = "@style/Theme.AppCompat.Light")]
    public class SelectLocationActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Load the UI defined in Location.axml
            SetContentView(Resource.Layout.selectLocation);

        }
    }
}