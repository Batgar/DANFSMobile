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
			AllShips = new List<ShipToken>();
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
				DefinesPresentationContext = false
			};

			DefinesPresentationContext = true;

			SearchController.SearchBar.SizeToFit();

			this.TableView.TableHeaderView = SearchController.SearchBar;
		}

		UISearchController SearchController { get; set;}

		private async void LoadShips()
		{
			var dataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess> ();

			//this.AllShips = await dataAccess.GetAllShips ();

			//this.ShipCount = dataAccess.GetAllShipCounts();

			this.AllShips.Clear();

			this.AllShips.AddRange(await dataAccess.GetAllShips());

			//It may have taken a while for the ships to load. Refresh the list.
			this.TableView.ReloadData();
		}

		//int ShipCount { get; set;}

		List<ShipToken> AllShips { get; set;}
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
			return Ships.Count;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = this.TableView.DequeueReusableCell ("ShipCellReuseIdentifier");

			/*if (indexPath.Row >= Ships.Count)
			{
				//Fetch the next 100 chunk and append into AllShips.
				var dataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess>();

				AllShips.AddRange(dataAccess.GetAllShipChunk(AllShips.Count, 100));
			}*/

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

