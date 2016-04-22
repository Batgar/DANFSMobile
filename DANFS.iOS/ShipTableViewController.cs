using System;
using UIKit;
using DANFS.Services;
using System.Collections.Generic;

namespace DANFS.iOS
{
	[Foundation.Register ("ShipTableViewController")]
	public class ShipTableViewController : UITableViewController
	{
		public ShipTableViewController (IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			LoadShips (); //This loads 480KB of JSON and processes 12,000 entries into an IList<IShipToken>
		}

		private async void LoadShips()
		{
			var dataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess> ();

			this.Ships = await dataAccess.GetAllShips ();

			//It may have taken a while for the ships to load. Refresh the list.
			this.TableView.ReloadData();
		}

		IList<ShipToken> Ships {get; set;}


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

			//Take the sender, find the UITableViewCell and get the index path for it.

			var indexPath = this.TableView.IndexPathForSelectedRow;

			var shipViewController = segue.DestinationViewController as ShipViewController;
			if (shipViewController != null) {
				shipViewController.Ship = this.Ships [indexPath.Row];
			}
		}
	}
}

