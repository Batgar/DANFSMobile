using System;
using Android.Widget;
using DANFS.Services;
using Android.App;
using System.Collections.Generic;
using Android.Views;

namespace DANFS.Android
{
	public class ShipListAdapter : BaseAdapter<IShipToken>
	{
		public ShipListAdapter (Activity context) : base() 
		{
			this.Context = context;

			//Fire and forget async data load!
			LoadData ();
		}

		private async void LoadData()
		{
			//Start loading the ShipList...
			var dataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess>();
			Ships = await dataAccess.GetAllShips ();
			this.NotifyDataSetChanged ();
		}

		IList<IShipToken> Ships { get; set; }

		Activity Context {get; set;}

		public override IShipToken this[int index] {
			get {
				if (Ships != null) {
					return Ships [index];
				}
				return null;
			}
		}

		public override int Count {
			get {
				if (Ships != null) {
					return Ships.Count;
				}
				return 0;
			}
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = Context.LayoutInflater.Inflate(Android.Resource.Layout.ShipListItem, null);
			view.FindViewById<TextView>(Android.Resource.Id.ShipTitleLabel).Text = Ships[position].Title;
			return view;
		}  
	}
}

