using System;
using System.Collections.Generic;
using System.ServiceModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace MyFestivalApp
{
    [Activity(Label = "Search By Date", Theme = "@style/Theme.AppCompat.Light")]
    public class SearchActivity : Activity
    {
		DateTime _date;
		ImageButton _btnBack;
        Spinner _spnLocation;
        private DataTransferProcClient _client;
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://10.0.2.2:3190/DataTransferProc.svc");
        TextView _getTextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.search);

            InitializeDataCounty();

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

			_spnLocation = FindViewById<Spinner> (Resource.Id.spnLocation);
            _getTextView = FindViewById<TextView>(Resource.Id.getTextView);
        }
            
		#region Date
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
		#endregion

        #region Spinners

        #region Populating Location Spinner with counties

            #region InitializeDataCounty
            private void InitializeDataCounty()
            {
                BasicHttpBinding binding = CreateBasicHttpCounty();
                _client = new DataTransferProcClient(binding, EndPoint);
                _client.GetCountiesDataCompleted += ClientOnDataTransferProcCompleted;
                _client.GetCountiesDataAsync();
                //_client.Close();
            }
            #endregion

            #region CreateBasicHttp
            private static BasicHttpBinding CreateBasicHttpCounty()
            {
                var binding = new BasicHttpBinding()
                {
                    Name = "basicHttpBinding",
                    MaxReceivedMessageSize = 67108864,
                };

                binding.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxArrayLength = 2147483646,
                    MaxStringContentLength = 5242880,
                };

                var timeout = new TimeSpan(0, 1, 0);
                binding.SendTimeout = timeout;
                binding.OpenTimeout = timeout;
                binding.ReceiveTimeout = timeout;
                return binding;
            }
            #endregion

            #region ClientOnDataTransferProcCompleted
        //test connection to wcf, if doesn't work, you'll get an error
        private void ClientOnDataTransferProcCompleted(object sender, GetCountiesDataCompletedEventArgs getCountiesDataCompletedEventArgs)
        {

            string msg = null;

            if (getCountiesDataCompletedEventArgs.Error != null)
            {
                msg = getCountiesDataCompletedEventArgs.Error.Message;
                msg += getCountiesDataCompletedEventArgs.Error.InnerException;
                RunOnUiThread(() => _getTextView.Text = msg);
            }
            else if (getCountiesDataCompletedEventArgs.Cancelled)
            {
                msg = "Request was cancelled.";
                RunOnUiThread(() => _getTextView.Text = msg);
            }
            else
            {
                msg = getCountiesDataCompletedEventArgs.Result.ToString();
                TestAndroid.Festivalwrapper testHolder = getCountiesDataCompletedEventArgs.Result;
                List<string> holder = new List<string>();

                foreach (TestAndroid.CountyVM item in testHolder.CountyList)
                {
                    holder.Add(item.Name);
                }

                RunOnUiThread(() => _spnLocation.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, holder));
            }
        }
        #endregion*/

        #endregion

        #endregion

    }
}