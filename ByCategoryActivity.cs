using System.ServiceModel;
using Android.App;
using Android.OS;
using Android.Widget;
namespace MyFestivalApp
{
    [Activity(Label = "Search By Category", Theme = "@style/Theme.AppCompat.Light")]
	public class ByCategoryActivity : Activity
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://192.168.1.1:3190/DataTransferProc.svc");
        private DataTransferProcClient _client;
        private ListView _listViewC;
        private ArrayAdapter _adapterC;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Load the UI defined in Location.axml
            SetContentView(Resource.Layout.category);

            var types = new[]
            {
                "Family Fun", "Youth", "Adults"
            };

            _listViewC = FindViewById<ListView>(Resource.Id.listView);
            _adapterC = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, types);
            _listViewC.Adapter = _adapterC;
        }
    }
}
