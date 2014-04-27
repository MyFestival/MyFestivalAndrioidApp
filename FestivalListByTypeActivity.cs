using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MyFestivalApp
{
	[Activity(Label = "Festivals with Selected Type", Theme = "@style/Theme.AppCompat.Light")]
	public class FestivalListByTypeActivity : Activity
    {
        private DataTransferProcClient _client;
        TextView _getFestivalsTextView;
        ListView _listView;
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://10.0.2.2:3190/DataTransferProc.svc");

        #region OnCreate
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            InitializeDataFestivalByTownId();
            // Load the UI defined in festival.axml
            SetContentView(Resource.Layout.festivallist);

            _listView = FindViewById<ListView>(Resource.Id.lvFestList);
            //_listView.OnItemClickListener = this;
            _listView.FastScrollEnabled = true;

            _getFestivalsTextView = FindViewById<TextView>(Resource.Id.getFestList);
        }
        #endregion

        #region InitializeDataFestival
        private void InitializeDataFestivalByTownId()
        {
            var id = Intent.GetStringExtra("Name");
            int value;
            int.TryParse(id, out value);

            if (value != null)
            {
                BasicHttpBinding binding = CreateBasicHttp();
                _client = new DataTransferProcClient(binding, EndPoint);
                _client.GetFestListDataByTownIdCompleted += ClientOnDataTransferProcCompleted;
                _client.GetFestListDataByTownIdAsync(value);
            }
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
        private void ClientOnDataTransferProcCompleted(object sender, GetFestListDataByTownIdCompletedEventArgs getFestListDataByTownIdCompletedEventArgs)
        {
            string msg = null;

            if (getFestListDataByTownIdCompletedEventArgs.Error != null)
            {
                msg = getFestListDataByTownIdCompletedEventArgs.Error.Message;
                msg += getFestListDataByTownIdCompletedEventArgs.Error.InnerException;
                RunOnUiThread(() => _getFestivalsTextView.Text = msg);
            }
            else if (getFestListDataByTownIdCompletedEventArgs.Cancelled)
            {
                msg = "Request was cancelled.";
                RunOnUiThread(() => _getFestivalsTextView.Text = msg);
            }
            else
            {
                msg = getFestListDataByTownIdCompletedEventArgs.Result.ToString();
                TestAndroid.Festivalwrapper testHolder = getFestListDataByTownIdCompletedEventArgs.Result;
                List<string> holder = testHolder.FestivalList.Select(item => item.FestivalName).ToList();

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