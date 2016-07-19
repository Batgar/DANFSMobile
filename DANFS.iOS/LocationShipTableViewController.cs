using System;
using System.Collections.Generic;
using DANFS.Services;
using System.Linq;
using UIKit;

namespace DANFS.iOS
{
	public partial class LocationShipTableViewController : UITableViewController, IUISearchResultsUpdating
	{
		public LocationShipTableViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var dataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess>();
			ShipDateList = dataAccess.GetShipListByLocation(this.LocationName);

			var searchController = new UISearchController((UIViewController)null)
			{
				WeakDelegate = this,
				DimsBackgroundDuringPresentation = false,
				WeakSearchResultsUpdater = this
			};

			searchController.SearchBar.SizeToFit();

			this.TableView.TableHeaderView = searchController.SearchBar;

			// Perform any additional setup after loading the view, typically from a nib.
		
		}

		List<shipLocationDate> ShipDateList { get; set;}

		public string LocationName { get; set;}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			if (SearchDateList != null)
			{
				return SearchDateList.Count;
			}
			return ShipDateList.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("LocationShipCell");
			cell.TextLabel.Text = SearchDateList != null ? SearchDateList[indexPath.Row].shipID : ShipDateList[indexPath.Row].shipID;
			cell.DetailTextLabel.Text = ShipDateList[indexPath.Row].startdate;
			return cell;
		}

		List<shipLocationDate> SearchDateList { get; set;}

		public void UpdateSearchResultsForSearchController(UISearchController searchController)
		{
			if (string.IsNullOrEmpty(searchController.SearchBar.Text))
			{
				SearchDateList = null;
				TableView.ReloadData();
				return;
			}

			SearchDateList = ShipDateList.Where(r => r.locationname.Contains(searchController.SearchBar.Text)).ToList();
			TableView.ReloadData();
		}
	}
}


