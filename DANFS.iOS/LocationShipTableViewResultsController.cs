using System;
using System.Collections.Generic;
using DANFS.Services;
using Foundation;
using System.Linq;
using UIKit;

namespace DANFS.iOS
{
	[Register("LocationShipTableViewResultsController")]
	public class LocationShipTableViewResultsController : UITableViewController, IUISearchResultsUpdating
	{
		public LocationShipTableViewResultsController(IntPtr handle) : base(handle)
		{
			SearchShipDates = new List<shipLocationDate>();
		}


		internal List<shipLocationDate> AllShipDates { get; set; }

		List<shipLocationDate> SearchShipDates { get; set;}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return SearchShipDates.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("LocationShipCell");
			cell.TextLabel.Text = SearchShipDates[indexPath.Row].shipID;
			cell.DetailTextLabel.Text = SearchShipDates[indexPath.Row].startdate;
			return cell;
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.DestinationViewController is ShipDocumentViewController)
			{
				var shipDate = SearchShipDates[TableView.IndexPathForSelectedRow.Row];
				var destinationController = segue.DestinationViewController as ShipDocumentViewController;
				destinationController.ShipId = shipDate.shipID;
				destinationController.LocationGuidToHighlight = shipDate.locationguid;
			}

		}

		public void UpdateSearchResultsForSearchController(UISearchController searchController)
		{
			UpdateListFilter(searchController);
			SearchController = searchController;
		}

		UISearchController SearchController { get; set;}

		private void UpdateListFilter(UISearchController searchController)
		{
			if (string.IsNullOrEmpty(searchController.SearchBar.Text))
			{
				SearchShipDates = AllShipDates;
			}
			else
			{
				SearchShipDates = AllShipDates.Where(r => r.shipID.ToLower().Contains(searchController.SearchBar.Text.ToLower())).ToList();
			}

			TableView.ReloadData();
		}
	}


}

