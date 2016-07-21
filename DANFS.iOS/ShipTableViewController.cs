using System;
using UIKit;
using DANFS.Services;
using System.Collections.Generic;
using System.Linq;

namespace DANFS.iOS
{
	[Foundation.Register ("ShipTableViewController")]
	public class ShipTableViewController : UITableViewController, IUISearchResultsUpdating
	{
		public ShipTableViewController (IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			LoadShips (); //This loads 480KB of JSON and processes 12,000 entries into an IList<IShipToken>

			SearchController = new UISearchController((UIViewController)null)
			{
				WeakDelegate = this,
				DimsBackgroundDuringPresentation = false,
				WeakSearchResultsUpdater = this,
				//HidesNavigationBarDuringPresentation = false
				DefinesPresentationContext = true
			};

			SearchController.SearchBar.SizeToFit();

			this.TableView.TableHeaderView = SearchController.SearchBar;
		}

		UISearchController SearchController { get; set;}

		private async void LoadShips()
		{
			var dataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess> ();

			this.AllShips = await dataAccess.GetAllShips ();

			//It may have taken a while for the ships to load. Refresh the list.
			this.TableView.ReloadData();
		}

		IList<ShipToken> AllShips { get; set;}
		IList<ShipToken> SearchShips { get; set;}

		IList<ShipToken> Ships
		{
			get
			{
				if (SearchShips != null)
				{
					return SearchShips;
				}
				return AllShips;
			}
		}


		public override nint RowsInSection (UITableView tableView, nint section)
		{
			if (this.Ships != null) {
				return this.Ships.Count;
			}
			return 0;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			if (this.Ships != null) {
				return 1;
			}
			return 0;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = this.TableView.DequeueReusableCell ("ShipCellReuseIdentifier");

			cell.TextLabel.Text = this.Ships [indexPath.Row].Title;

			return cell;
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			base.PrepareForSegue (segue, sender);

			SearchController.SearchBar.ResignFirstResponder();

			//Take the sender, find the UITableViewCell and get the index path for it.

			var indexPath = this.TableView.IndexPathForSelectedRow;

			var shipViewController = segue.DestinationViewController as ShipViewController;
			if (shipViewController != null) {
				shipViewController.Ship = this.Ships [indexPath.Row];
			}
		}

		public void UpdateSearchResultsForSearchController(UISearchController searchController)
		{
			if (string.IsNullOrEmpty(searchController.SearchBar.Text))
			{
				SearchShips = null;
				TableView.ReloadData();
				return;
			}

			SearchShips = AllShips.Where(r => r.Title.Contains(searchController.SearchBar.Text)).ToList();
			TableView.ReloadData();
		}
	}
}

