// This file has been autogenerated from a class added in the UI designer.

using System;
using DANFS.Services;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace DANFS.iOS
{
	public partial class ShipViewLocationTableViewController : UITableViewController
	{
		public ShipViewLocationTableViewController (IntPtr handle) : base (handle)
		{
		}

		public ShipToken Ship { get; internal set; }

		public List<ShipLocationHistoryResult> ShipLocations { get; private set; }


		async void RefreshWithLocations()
		{
			//Load all the locations for this ship.
			var dataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess>();

			ShipLocations = await dataAccess.GetRawGeolocationsForShip(this.Ship);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			RefreshWithLocations();
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = this.TableView.DequeueReusableCell("LocationCell");

			var shipLocation = ShipLocations[indexPath.Row];

			cell.TextLabel.Text = shipLocation.Location;

			cell.DetailTextLabel.Text = string.Empty;

			cell.DetailTextLabel.Text = shipLocation.PossibleStartDate?.ToShortDateString();

			return cell;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return ShipLocations != null ? 1 : 0;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			if (ShipLocations != null)
			{
				return ShipLocations.Count;
			}
			return 0;
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.DestinationViewController is ShipDocumentViewController)
			{
				var lastSelectedLocation = this.ShipLocations[this.TableView.IndexPathForSelectedRow.Row];
				var shipDocumentViewController = segue.DestinationViewController as ShipDocumentViewController;
				shipDocumentViewController.ShipId = this.Ship.ID;
				shipDocumentViewController.LocationGuidToHighlight = lastSelectedLocation.LocationGuid;
			}
			base.PrepareForSegue(segue, sender);
		}
	}
}
