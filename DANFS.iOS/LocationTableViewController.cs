using System;
using System.Collections.Generic;
using DANFS.Services;
using System.Linq;
using UIKit;

namespace DANFS.iOS
{
	public partial class LocationTableViewController : UITableViewController, IUISearchResultsUpdating
	{
		public LocationTableViewController(IntPtr handle) : base(handle)
		{
			
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			var dataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess>();
			Locations = dataAccess.GetUniqueLocationList();

			var searchController = new UISearchController((UIViewController)null)
			{
				WeakDelegate = this,
				DimsBackgroundDuringPresentation = false,
				WeakSearchResultsUpdater = this
			};

			searchController.SearchBar.SizeToFit();

			this.TableView.TableHeaderView = searchController.SearchBar;
		}

		List<string> Locations { get; set;}

		List<string> SearchLocationList { get; set;}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("LocationCell");
			cell.TextLabel.Text = SearchLocationList != null ? SearchLocationList[indexPath.Row] : Locations[indexPath.Row];
			return cell;
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return SearchLocationList != null ? SearchLocationList.Count : Locations.Count;
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			if (segue.DestinationViewController is LocationShipTableViewController)
			{
				var locationShipTableViewController = segue.DestinationViewController as LocationShipTableViewController;
				locationShipTableViewController.LocationName = SearchLocationList != null ? SearchLocationList[TableView.IndexPathForSelectedRow.Row] : Locations[TableView.IndexPathForSelectedRow.Row];
			}
		}

		public void UpdateSearchResultsForSearchController(UISearchController searchController)
		{
			if (string.IsNullOrEmpty(searchController.SearchBar.Text))
			{
				SearchLocationList = null;
				TableView.ReloadData();
				return;
			}

			SearchLocationList = Locations.Where(r => r.Contains(searchController.SearchBar.Text)).ToList();
			TableView.ReloadData();
		}
	}
}


