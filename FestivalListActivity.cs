using System.ServiceModel;
using System.Text;
using Android.App;
using Android.OS;
using Android.Widget;

namespace MyFestivalApp
{
    [Activity(Label = "S", Theme = "@style/Theme.AppCompat.Light")]
    public class FestivalListActivity : Activity
    {
		//private DataTransferProcClient _client;
		//TextView _getFestivalTextView;
		//ListView _listView;
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://10.0.2.2:3190/DataTransferProc.svc");

        //private ShareActionProvider actionProvider;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Load the UI defined in festival.axml
            SetContentView(Resource.Layout.festivallist);
        }
    }
}