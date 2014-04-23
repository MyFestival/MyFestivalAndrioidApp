using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
	[Activity(Label = "Selected County", Theme = "@style/Theme.AppCompat.Light")]
    public class SelectLocationActivity : Activity
    {
		private DataTransferProcClient _client;
        TextView _getTownsTextView;
        ListView _listView;
		public static readonly EndpointAddress EndPoint = new EndpointAddress("http://10.0.2.2:3190/DataTransferProc.svc");

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.selectLocation);
			InitializeDataTown();

            _listView = FindViewById<ListView>(Resource.Id.lvTowns);
            //_listView.OnItemClickListener = this;
            _listView.FastScrollEnabled = true;

            _getTownsTextView = FindViewById<TextView>(Resource.Id.getTownsTextView);
        }

		#region InitializeDataTown
		private void InitializeDataTown()
		{
			BasicHttpBinding binding = CreateBasicHttp();
			_client = new DataTransferProcClient(binding, EndPoint);
			_client.GetCountiesDataCompleted += ClientOnDataTransferProcCompleted;
			_client.GetCountiesDataAsync();
			//_client.Close ();
		}
		#endregion

		#region CreateBasicHttp
		private static BasicHttpBinding CreateBasicHttp()
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
                RunOnUiThread(() => _getTownsTextView.Text = msg);
            }
            else if (getCountiesDataCompletedEventArgs.Cancelled)
            {
                msg = "Request was cancelled.";
                RunOnUiThread(() => _getTownsTextView.Text = msg);
            }
            else
            {
                msg = getCountiesDataCompletedEventArgs.Result.ToString();
                TestAndroid.Festivalwrapper testHolder = getCountiesDataCompletedEventArgs.Result;
                List<string> holder = testHolder.CountyList.Select(item => item.Name).ToList();

                /*foreach (TestAndroid.CountyVM item in TestHolder.CountyList)
				{
					holder.Add (item.Name);
				}*/

                RunOnUiThread(() => _listView.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, holder));
            }
        }
        #endregion
    }
}