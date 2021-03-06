// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using DANFS.Services;
using System.Linq;
using SQLite.Net.Interop;
using MapKit;
using CoreGraphics;

namespace DANFS.iOS
{
	public partial class ShipViewController : UIViewController
	{
		public ShipViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.NavigationItem.Title = this.Ship.Title;

		}

		public ShipToken Ship { get; set; }


		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);

			if (segue.DestinationViewController is LocationMapViewController)
			{
				var controller = segue.DestinationViewController as LocationMapViewController;
				controller.Ship = this.Ship;
			}
			else if (segue.DestinationViewController is ShipViewLocationTableViewController)
			{
				var controller = segue.DestinationViewController as ShipViewLocationTableViewController;
				controller.Ship = this.Ship;
			}
		}

		partial void showComponent(NSObject sender)
		{
			var segmentedControl = sender as UISegmentedControl;
			switch (segmentedControl.SelectedSegment)
			{
				case 0:
					this.mapContainerView.Hidden = true;
					this.locationsContainerView.Hidden = false;
					break;

				case 1:
					this.mapContainerView.Hidden = false;
					this.locationsContainerView.Hidden = true;
					break;
			}

		}

	}


}
