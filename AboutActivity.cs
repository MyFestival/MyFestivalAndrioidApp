using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace MyFestivalApp
{
    [Activity(Label = "About MyFestival", Theme = "@style/Theme.AppCompat.Light")]
    public class AboutActivity : Activity
    {
        ImageButton _btnBack;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Load the UI defined in Location.axml
            SetContentView(Resource.Layout.about);

            #region btnBack

            _btnBack = FindViewById<ImageButton>(Resource.Id.btnByLocation);
            _btnBack.Click += (sender, e) =>
            {
                var back = new Intent(this, typeof(Activity1));
                StartActivity(back);
            };

            #endregion
            
        }
    }
}
