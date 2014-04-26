using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace MyFestivalApp
{
    [Activity(Label = "Search By Location", Theme = "@style/Theme.AppCompat.Light")]
	public class ByLocationActivity : Activity, ListView.IOnItemClickListener
	{
		//private SearchView _searchView;
        private ListView _listView;
		//private ArrayAdapter _adapter;
        private DataTransferProcClient _client;
        private TextView _getCountiesTextView;
		public static readonly EndpointAddress EndPoint = new EndpointAddress("http://10.0.2.2:3190/DataTransferProc.svc");

        #region onCreate
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.location);
            InitializeDataCounty();

			#region hard coded array list of counties
            /*var counties = new[]
            {
                "Antrim", "Armagh", "Carlow", "Cavan", "Clare", "Cork", "Donegal", "Down","Derry",
                "Dublin", "Fermanagh", "Galway", "Kerry", "Kildare", "Kilkenny", "Laois", "Leitrim",
                "Limerick", "Longford", "Louth", "Mayo", "Meath", "Monaghan", "Offaly",
                "Roscommon", "Sligo", "Tipp", "Tyrone", "Waterford", "Westmeath", "Wexford", "Wicklow"
            };*/

			//_adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, counties);
			//_listView.Adapter = _adapter;
			#endregion

			_listView = FindViewById<ListView>(Resource.Id.listView);
			_listView.OnItemClickListener = this;
            _listView.FastScrollEnabled = true;

            _getCountiesTextView = FindViewById<TextView>(Resource.Id.getCountiesTextView);

        }
		#endregion

        #region InitializeDataCounty
        private void InitializeDataCounty()
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
				msg+= getCountiesDataCompletedEventArgs.Error.InnerException;
				RunOnUiThread(() => _getCountiesTextView.Text = msg);
            }
            else if (getCountiesDataCompletedEventArgs.Cancelled)
            {
                msg = "Request was cancelled.";
				RunOnUiThread(() => _getCountiesTextView.Text = msg);
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

		public void OnItemClick(AdapterView parent, View view, int position, long id)
		{
			var selectedValue = parent.GetItemIdAtPosition(position);
			//InitializeDataTownById(int position);
			var Intent = new Intent(this, typeof(SelectLocationActivity));
            // selectedValue should already be a string but...
            Intent.PutExtra("Name", selectedValue.ToString());
			StartActivity(Intent);
		}
			
        #region Search List
		//public override bool OnCreateOptionsMenu(IMenu menu)
		//{
		//	MenuInflater.Inflate(Resource.Menu.search, menu);

		//	var item = menu.FindItem(Resource.Id.action_search);
		//	var backbutton = menu.FindItem(Resource.Id.action_back);

		//	var searchview = MenuItemCompat.GetActionView(item);
		//	_searchView = searchview.JavaCast<SearchView>();

		//	_searchView.QueryTextChange += (s, e) => _listView.Filter.InvokeFilter(e.NewText);

		//	_searchView.QueryTextSubmit += (s, e) =>
		//	{
		//		Toast.MakeText(this, "Search for: " + e.Query, ToastLength.Short).Show();
		//		e.Handled = true;
		//	};
		//	return true;
		/*}*/
		#endregion
    }
}
