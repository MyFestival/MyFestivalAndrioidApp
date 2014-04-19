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
    [Activity(Label = "Search By Location", Theme = "@style/Theme.AppCompat.Light")]
	public class ByLocationActivity : ActionBarActivity
    {
        private SearchView _searchView;
        private ListView _listView;
        private ArrayAdapter _adapter;
        private DataTransferProcClient _client;
        private TextView _getCountiesTextView;
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://192.168.1.1:3190/DataTransferProcClient.svc");

        #region onCreate
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Load the UI defined in Location.axml
            SetContentView(Resource.Layout.location);

            InitializeDataCounty();

            //hard coded array
            /*var counties = new[]
            {
                "Antrim", "Armagh", "Carlow", "Cavan", "Clare", "Cork", "Donegal", "Down","Derry",
                "Dublin", "Fermanagh", "Galway", "Kerry", "Kildare", "Kilkenny", "Laois", "Leitrim",
                "Limerick", "Longford", "Louth", "Mayo", "Meath", "Monaghan", "Offaly",
                "Roscommon", "Sligo", "Tipp", "Tyrone", "Waterford", "Westmeath", "Wexford", "Wicklow"
            };*/

            _listView = FindViewById<ListView>(Resource.Id.listView);
            //_adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, counties);
            _listView.Adapter = _adapter;

            _getCountiesTextView = FindViewById<TextView>(Resource.Id.getCountiesTextView);

        }
        #endregion

        #region Search List
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.search, menu);

            var item = menu.FindItem(Resource.Id.action_search);
            //var backbutton = menu.FindItem(Resource.Id.action_back);

            var searchview = MenuItemCompat.GetActionView(item);
            _searchView = searchview.JavaCast<SearchView>();

            _searchView.QueryTextChange += (s, e) => _adapter.Filter.InvokeFilter(e.NewText);

            _searchView.QueryTextSubmit += (s, e) =>
            {
                Toast.MakeText(this, "Search for: " + e.Query, ToastLength.Short).Show();
                e.Handled = true;
            };
            return true;
        }
        #endregion


        private void InitializeDataCounty()
        {
            BasicHttpBinding binding = CreateBasicHttp();
            _client = new DataTransferProcClient(binding, EndPoint);
            _client.GetCountiesDataAsync();

            var lstWebServiceMonoData = new List<GetCountiesDataCompletedEventArgs>();
            
            _client.GetCountiesDataCompleted += ClientOnDataTransferProcCompleted;

            var mainListView = FindViewById<ListView>(Resource.Id.listView);
	            mainListView.Adapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1);
        }

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

        private void ClientOnDataTransferProcCompleted(object sender, GetCountiesDataCompletedEventArgs getCountiesDataCompletedEventArgs)
        {
            string msg = null;

            if (getCountiesDataCompletedEventArgs.Error != null)
            {
                msg = getCountiesDataCompletedEventArgs.Error.Message;
            }
            else if (getCountiesDataCompletedEventArgs.Cancelled)
            {
                msg = "Request was cancelled.";
            }
            else
            {
                msg = getCountiesDataCompletedEventArgs.Result.ToString();
            }
            RunOnUiThread(() => _getCountiesTextView.Text = msg);
        }
    }
}
