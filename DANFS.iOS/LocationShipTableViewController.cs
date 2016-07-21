using System;
using System.Collections.Generic;
using DANFS.Services;
using System.Linq;
using UIKit;
using Foundation;

namespace DANFS.iOS
{
	public partial class LocationShipTableViewController : UITableViewController, IUISearchControllerDelegate
	{
		public LocationShipTableViewController(IntPtr handle) : base(handle)
		{
			// Perform any additional setup after loading the view, typically from a nib.
			FilterDateTime = DateTime.Parse("June 3, 1942");
			FilterDayRange = 10;
			FilterApplied = false;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


			var dataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess>();
			AllShipDates = dataAccess.GetShipListByLocation(this.LocationName);

			UIStoryboard sb = UIStoryboard.FromName("Main", null);
			var mainNavigationController = (sb.InstantiateViewController("LocationShipResultsViewID") as UINavigationController);
			ResultsController = mainNavigationController.ViewControllers[0] as LocationShipTableViewResultsController;
			ResultsController.AllShipDates = AllShipDates;
			//this.NavigationController.DefinesPresentationContext = true;
			//var resultsController = new LocationShipTableViewResultsController();

			SearchController = new UISearchController(mainNavigationController)
			{
				WeakDelegate = this,
				DimsBackgroundDuringPresentation = false,
				WeakSearchResultsUpdater = ResultsController,
				HidesNavigationBarDuringPresentation = true,
				DefinesPresentationContext = false
			};

			this.DefinesPresentationContext = true;


			SearchController.SearchBar.SizeToFit();

			this.TableView.TableHeaderView = SearchController.SearchBar;


		}

		LocationShipTableViewResultsController ResultsController { get; set;}

		[Export("presentSearchController:")]
		public void PresentSearchController(UISearchController searchController)
		{
			ResultsController.AllShipDates = this.SearchShipDates != null ? this.SearchShipDates : this.AllShipDates;
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
			return 1;
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return CurrentShipDates.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("LocationShipCell");
			cell.TextLabel.Text = CurrentShipDates[indexPath.Row].shipID;
			cell.DetailTextLabel.Text = CurrentShipDates[indexPath.Row].startdate;
			return cell;
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			if (segue.DestinationViewController is ShipDocumentViewController)
			{
				var shipDate = CurrentShipDates[TableView.IndexPathForSelectedRow.Row];
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
				UpdateListFilter();
			}
		}

		public List<shipLocationDate> SearchShipDates { get; private set; }

		private void UpdateListFilter()
		{
			


			if (this.FilterApplied)
			{
				SearchShipDates = AllShipDates.Where(r =>
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
			else
			{
				SearchShipDates = null;
			}

			TableView.ReloadData();
		}

		private DateTime FilterDateTime { get; set; }
		private int FilterDayRange { get; set; }
		private bool FilterApplied { get; set; }
		




	}
}


