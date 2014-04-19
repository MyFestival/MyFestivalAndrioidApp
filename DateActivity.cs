using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace MyFestivalApp
{
    [Activity(Label = "Search By Date", Theme = "@style/Theme.AppCompat.Light")]
    public class DateActivity : Activity
    {
        DateTime _date;
        ImageButton _btnBack;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.date);

            #region startDate

            var _btnStartDate = FindViewById<Button>(Resource.Id.startDate);

            _btnStartDate.Click += delegate
            {
                ShowDialog(0);
            };
            _date = DateTime.Today;
            _btnStartDate.Text = _date.ToString("d");

            #endregion

            #region endDate

            var _btnEndDate = FindViewById<Button>(Resource.Id.endDate);

            _btnEndDate.Click += delegate
            {
                ShowDialog(0);
            };

            _date = DateTime.Today;
            _btnEndDate.Text = _date.ToString("d");

            #endregion

            #region btnBack

            _btnBack = FindViewById<ImageButton>(Resource.Id.btnByLocation);
            _btnBack.Click += (sender, e) =>
            {
                var back = new Intent(this, typeof(Activity1));
                StartActivity(back);
            };

            #endregion

        }

        protected override Dialog OnCreateDialog(int id)
        {
            return new DatePickerDialog(this, HandleDateSet, _date.Year, _date.Month - 1, _date.Day);
        }

        void HandleDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            #region StartDate
            _date = e.Date;
            var _btnStartDate = FindViewById<Button>(Resource.Id.startDate);
            _btnStartDate.Text = _date.ToString("d");
            #endregion

            #region EndDate
            _date = e.Date;
            var _btnEndDate = FindViewById<Button>(Resource.Id.endDate);
            _btnEndDate.Text = _date.ToString("d");
            #endregion
        }

    }
}
