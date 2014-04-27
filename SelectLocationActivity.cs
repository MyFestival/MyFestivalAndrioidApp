using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Android.App;
using Android.Content;
using Android.OS;
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
	    //private int id;

        protected override void OnCreate(Bundle bundle)
        {
            //var id = Intent.GetStringExtra("position");

			base.OnCreate(bundle);
			InitializeDataTownById();
			SetContentView(Resource.Layout.selectLocation);
			
            _listView = FindViewById<ListView>(Resource.Id.lvTowns);
            //_listView.OnItemClickListener = this;
            _listView.FastScrollEnabled = true;

            _getTownsTextView = FindViewById<TextView>(Resource.Id.getTownsTextView);
        }

		#region InitializeDataTown
		private void InitializeDataTownById()
		{
			var id = Intent.GetStringExtra("Name");
			int value;
			int.TryParse(id, out value);

			if (value != null)
			{
				BasicHttpBinding binding = CreateBasicHttp();
				_client = new DataTransferProcClient(binding, EndPoint);
				_client.GetTownDataByCountyCompleted += ClientOnDataTransferProcCompleted;
				_client.GetTownDataByCountyAsync(value);
			}
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
		private void ClientOnDataTransferProcCompleted(object sender, GetTownDataByCountyCompletedEventArgs getTownDataByCountyCompletedEventArgs)
        {
            string msg = null;

            if (getTownDataByCountyCompletedEventArgs.Error != null)
            {
				msg = getTownDataByCountyCompletedEventArgs.Error.Message;
				msg += getTownDataByCountyCompletedEventArgs.Error.InnerException;
                RunOnUiThread(() => _getTownsTextView.Text = msg);
            }
			else if (getTownDataByCountyCompletedEventArgs.Cancelled)
            {
                msg = "Request was cancelled.";
                RunOnUiThread(() => _getTownsTextView.Text = msg);
            }
            else
            {
				msg = getTownDataByCountyCompletedEventArgs.Result.ToString();
				TestAndroid.Festivalwrapper testHolder = getTownDataByCountyCompletedEventArgs.Result;
				List<string> holder = testHolder.TownList.Select(item => item.Name).ToList();

                /*foreach (TestAndroid.CountyVM item in TestHolder.CountyList)
				{
					holder.Add (item.Name);
				}*/

                RunOnUiThread(() => _listView.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, holder));
            }
        }
        #endregion

        #region OnItemClick
        public void OnItemClick(AdapterView parent, View view, int position, long id)
		{
			var selectedValue = parent.GetItemIdAtPosition(position);
			//InitializeDataTownById(int position);
			var Intent = new Intent(this, typeof(FestivalListActivity));
            // selectedValue should already be a string but...
            Intent.PutExtra("Name", selectedValue.ToString());
			StartActivity(Intent);
        }
        #endregion

    }
}