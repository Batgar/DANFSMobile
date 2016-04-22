
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
using DANFS.Services;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace DANFS.Android
{
	[Activity (Label = "ShipLocationActivity")]			
	public class ShipLocationActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			//API key: AIzaSyCfm_xR9xztvnrG49r7QMcbotKwTzv19Lc

			base.OnCreate (savedInstanceState);

			base.SetContentView (Resource.Layout.ShipLocation);

			// Create your application here
			ShipLocationTextView = FindViewById<TextView>(Resource.Id.shipLocationTextView);

			/*MapFragment mapFrag = (MapFragment) FragmentManager.FindFragmentById(Resource.Id.map);
			Map = mapFrag.Map;
			if (Map != null) {
				Map.MapType = GoogleMap.MapTypeSatellite;
			}*/

			//Pull the Ship data we are supposed to show locations for.
			ShipToken = ShipToken.Deserialize(this.Intent.GetStringExtra("ShipToken"));

			RefreshData ();
		}

		GoogleMap Map {get; set;}

		ShipToken ShipToken {get; set;}

		private async void RefreshData()
		{
			var dataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess> ();
			var shipLocations = await dataAccess.GetRawGeolocationsForShip (ShipToken);

			foreach (var location in shipLocations) {
				ShipLocationTextView.Text += location.Location + "\n";
				/*if (location.LocationGeocodeResult != null) {
					//Only plot the first result.
					var result = location.LocationGeocodeResult.Results.FirstOrDefault();
					if (result == null) {
						continue;
					}
					var geometry = result.Geometry;
					if (geometry != null && geometry.Viewport != null) {
						var val = geometry.Viewport.Values.FirstOrDefault ();

						if (val == null) {
							continue;
						}

						var lat = val.Lat;
						var longitude = val.Long;

						MarkerOptions markerOpt1 = new MarkerOptions ();
						markerOpt1.SetPosition (new LatLng (lat, longitude));
						markerOpt1.SetTitle (location.Location);
						Map.AddMarker (markerOpt1);

					}
				}*/
			}
		}

		TextView ShipLocationTextView {get; set;}
	}
}

