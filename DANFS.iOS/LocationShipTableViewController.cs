using System;
using System.Collections.Generic;
using DANFS.Services;
using System.Linq;
using UIKit;
using Foundation;

namespace DANFS.iOS
{
	public partial class LocationShipTableViewController : UITableViewController, IUISearchResultsUpdating
	{
		public LocationShipTableViewController(IntPtr handle) : base(handle)
		{
			// Perform any additional setup after loading the view, typically from a nib.
			FilterDateTime = DateTime.Parse("June 3, 1942");
			FilterDayRange = 10;
			FilterApplied = false;
		}

		public List<string> LocationNames { get; internal set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


			var dataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess>();

			if (this.LocationNames == null)
			{
				AllShipDates = dataAccess.GetShipListByLocation(this.LocationName);
			}
			else
			{
				AllShipDates = dataAccess.GetShipListByMultipleLocations(this.LocationNames);
			}



			SearchController = new UISearchController((UIViewController)null)
			{
				DimsBackgroundDuringPresentation = false,
				WeakSearchResultsUpdater = this,
				HidesNavigationBarDuringPresentation = false,
				DefinesPresentationContext = false
			};

			this.DefinesPresentationContext = true;


			SearchController.SearchBar.SizeToFit();

			this.TableView.TableHeaderView = SearchController.SearchBar;


		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			this.NavigationController.Toolbar.Hidden = true;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			this.NavigationController.Toolbar.Hidden = false;
		}


		UISearchController SearchController { get; set;}

		List<shipLocationDate> AllShipDates { get; set;}

		List<shipLocationDate> CurrentShipDates
		{
			get
			{
				if (SearchShipDates != null)
				{
					return SearchShipDates;
				}
				return AllShipDates;
			}
		}


		public string LocationName { get; set;}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 2;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			switch (section)
			{
				case 0:
					return CurrentShipDates.Count(o => o.shiplocationdatetype == "namehistory");
				case 1:
					return CurrentShipDates.Count(o => o.shiplocationdatetype == "log");
			}
			throw new Exception("Invalid section row retrieval");
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			switch (section)
			{
				case 0:
					return "Ship Name History";
				case 1:
					return "Log";

			}
			throw new Exception("Invalid section title for header retrieval");
		}

		shipLocationDate GetShipLocationDateForSectionAndRow(int section, int rowIndex)
		{
			switch (section)
			{
				case 0:
				return CurrentShipDates.Where(o => o.shiplocationdatetype == "namehistory").ElementAt(rowIndex);
				case 1:
				return CurrentShipDates.Where(o => o.shiplocationdatetype == "log").ElementAt(rowIndex);
			}
			throw new Exception("Invalid section row data retrieval");
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("LocationShipCell");
			var cellData = GetShipLocationDateForSectionAndRow(indexPath.Section, indexPath.Row);
			cell.TextLabel.Text = cellData.shipID;
			cell.DetailTextLabel.Text = cellData.startdate;
			return cell;
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			if (segue.DestinationViewController is ShipDocumentViewController)
			{
				var shipDate = this.GetShipLocationDateForSectionAndRow(TableView.IndexPathForSelectedRow.Section, TableView.IndexPathForSelectedRow.Row);
				var destinationController = segue.DestinationViewController as ShipDocumentViewController;
				destinationController.ShipId = shipDate.shipID;
				destinationController.LocationGuidToHighlight = shipDate.locationguid;
			}
			else if (segue.DestinationViewController is FilterDateViewController)
			{
				var filterDateViewController = segue.DestinationViewController as FilterDateViewController;
				filterDateViewController.ChosenDateTime = this.FilterDateTime;
				filterDateViewController.DayRange = this.FilterDayRange;
			}
		}

		[Action("UnwindToLocationShipController:")]
		public void UnwindToLocationShipController(UIStoryboardSegue segue)
		{
			Console.WriteLine("We've unwinded!");

			if (segue.SourceViewController is FilterDateViewController)
			{
				var filterDateViewController = segue.SourceViewController as FilterDateViewController;

				if (filterDateViewController.ClearFilter)
				{
					this.FilterApplied = false;
				}
				else
				{
					this.FilterDateTime = filterDateViewController.ChosenDateTime;
					this.FilterDayRange = filterDateViewController.DayRange;
					this.FilterApplied = true;
				}
				UpdateSearch();
			}
		}

		public List<shipLocationDate> SearchShipDates { get; private set; }

		private void UpdateSearch()
		{
			if (string.IsNullOrEmpty(SearchController.SearchBar.Text))
			{
				SearchShipDates = null;
			}
			else
			{
				SearchShipDates = AllShipDates.Where(r => r.shipID.ToLower().Contains(SearchController.SearchBar.Text.ToLower())).ToList();
			}

			if (this.FilterApplied)
			{
				SearchShipDates = CurrentShipDates.Where(r =>
				{
					if (string.IsNullOrEmpty(r.startdate))
					{
						return false;
					}

					var startDate = DateTime.Parse(r.startdate).Date;
					var maxStartDate = this.FilterDateTime + TimeSpan.FromDays(this.FilterDayRange);
					var minStartDate = this.FilterDateTime - TimeSpan.FromDays(this.FilterDayRange);

					if (startDate < minStartDate ||
						startDate > maxStartDate)
					{
						return false;
					}
					else
					{
						return true;
					}

				}).ToList();
			}

			TableView.ReloadData();
		}

		public void UpdateSearchResultsForSearchController(UISearchController searchController)
		{
			UpdateSearch();
		}

		private DateTime FilterDateTime { get; set; }
		private int FilterDayRange { get; set; }
		private bool FilterApplied { get; set; }
		




	}
}


