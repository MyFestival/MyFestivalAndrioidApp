using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Emit;
using System.ServiceModel;
using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Java.Text;

namespace MyFestivalApp
{
	[Activity(Label = "Search By Festival Type", Theme = "@style/Theme.AppCompat.Light")]
	public class ByCategoryActivity : Activity
    {
		private ListView _listView;
		private DataTransferProcClient _client;
		private TextView _getFTypeTextView;
		public static readonly EndpointAddress EndPoint = new EndpointAddress("http://10.0.2.2:3190/DataTransferProc.svc");

		#region onCreate
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Load the UI defined in category.axml
			SetContentView(Resource.Layout.category);

			InitializeDataFestivalType();

			_listView = FindViewById<ListView>(Resource.Id.lvTypes);
			_listView.FastScrollEnabled = true;
			_getFTypeTextView = FindViewById<TextView>(Resource.Id.getTypeTextView);

		}
		#endregion

		#region InitializeDataFestivalType
		private void InitializeDataFestivalType()
		{
			BasicHttpBinding binding = CreateBasicHttp();
			_client = new DataTransferProcClient(binding, EndPoint);
			_client.GetFestTypeDataCompleted += ClientOnDataTransferProcCompleted;
			_client.GetFestTypeDataAsync();


			var lstWebServiceMonoData = new List<GetFestTypeDataCompletedEventArgs>();

			var mainListView = FindViewById<ListView>(Resource.Id.lvTypes);
			mainListView.Adapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1);

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
		private void ClientOnDataTransferProcCompleted(object sender, GetFestTypeDataCompletedEventArgs getFestTypeDataCompletedEventArgs)
		{

			string msg = null;

			if (getFestTypeDataCompletedEventArgs.Error != null)
			{
				msg = getFestTypeDataCompletedEventArgs.Error.Message;
				msg+= getFestTypeDataCompletedEventArgs.Error.InnerException;
				RunOnUiThread(() => _getFTypeTextView.Text = msg);
			}
			else if (getFestTypeDataCompletedEventArgs.Cancelled)
			{
				msg = "Request was cancelled.";
				RunOnUiThread(() => _getFTypeTextView.Text = msg);
			}
			else
			{
				msg = getFestTypeDataCompletedEventArgs.Result.ToString();
				TestAndroid.Festivalwrapper TestHolder = getFestTypeDataCompletedEventArgs.Result;
				List<string> holder = new List<string> ();

				foreach (TestAndroid.FestivalTypeVM item in TestHolder.FestivalTypeList)
				{
					holder.Add (item.FType);
				}

				RunOnUiThread(() => _listView.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, holder));
			}
		}
		#endregion
	}
}
