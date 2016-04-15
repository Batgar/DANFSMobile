
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DANFS.Android
{
	[Activity (Label = "ShipListActivity", MainLauncher = true, Icon = "@mipmap/icon")]		
	public class ShipListActivity : ListActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			AppBootstrapper.Initialize ();

			//Start loading the master ship list for the ListActivity.
			ListAdapter = new ShipListAdapter(this);
		}
	}
}

