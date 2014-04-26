using System;   
using System.ServiceModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Widget;
using TestAndroid;

namespace MyFestivalApp
{
    //[Activity(Label = "\t\t\t\t\t\tHome", Theme = "@style/Theme.AppCompat.Light")]
	[Activity(Label = "Home", Theme = "@style/Theme.AppCompat.Light")]
    public class Activity1 : Activity
    {
        private ImageButton _btnLocation, _btnCategory, _btnByDate, _btnAbout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            #region Location
            _btnLocation = FindViewById<ImageButton>(Resource.Id.btnByLocation);
            _btnLocation.Click += GetCountiesButtonOnClick;
            #endregion

            #region Category
            _btnCategory = FindViewById<ImageButton>(Resource.Id.btnCategory);
            _btnCategory.Click += GetCategoryButtonOnClick;
            #endregion

			#region Date
			_btnByDate = FindViewById<ImageButton>(Resource.Id.btnDate);
			_btnByDate.Click += SearchOnClick;
			#endregion

            #region About
            _btnAbout = FindViewById<ImageButton>(Resource.Id.btnAbout);
            _btnAbout.Click += GetAboutButtonOnClick;
            #endregion

        }

        #region County/Location Page
        private void GetCountiesButtonOnClick(object sender, EventArgs eventArgs)
        {
            //CountyVM data = new CountyVM();
            var loc = new Intent(this, typeof(ByLocationActivity));
            StartActivity(loc);
        }
        #endregion

        #region Category Page
        private void GetCategoryButtonOnClick(object sender, EventArgs eventArgs)
        {
            var cat = new Intent(this, typeof(ByCategoryActivity));
            StartActivity(cat);
        }
        #endregion

        #region Search Page
        private void SearchOnClick(object sender, EventArgs eventArgs)
        {
            var search = new Intent(this, typeof(SearchActivity));
            StartActivity(search);
        }
        #endregion

        #region about
        private void GetAboutButtonOnClick(object sender, EventArgs eventArgs)
        {
            var about = new Intent(this, typeof(AboutActivity));
            StartActivity(about);
        }
        #endregion
    }
}
