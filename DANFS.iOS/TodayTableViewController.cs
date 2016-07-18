using System;
using DANFS.Services;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using Foundation;

namespace DANFS.iOS
{
	public partial class TodayTableViewController : UITableViewController
	{
		public TodayTableViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			DataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess>();
			//Sections = DataAccess.GetTodayInNavyHistoryYearSections().ToList();
			All = DataAccess.GetTodayInNavyHistory();

			TableView.RowHeight = 65;
		}

		IDataAccess DataAccess { get; set;}

		//IList<string> Sections { get; set;}

		IList<shipdate> All { get; set;}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return All.Count; //DataAccess.GetTodayInNavyHistoryByYear(Sections[(int)section]).Count();
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1; //Sections.Count();
		}

		/*public override string[] SectionIndexTitles(UITableView tableView)
		{
			return Sections.ToArray();
		}*/

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = this.TableView.DequeueReusableCell("TodayCell");
			var shipEvent = All[indexPath.Row];

			cell.TextLabel.Font = UIFont.FromName("Courier New", 14);
			cell.TextLabel.Lines = 2;
			cell.TextLabel.AdjustsFontSizeToFitWidth = true;
			cell.TextLabel.AdjustsLetterSpacingToFitWidth = true;
			cell.TextLabel.MinimumFontSize = 8;

				//DataAccess.GetTodayInNavyHistoryByYear(Sections.ElementAt((int)indexPath.Section)).ElementAt(indexPath.Row);
			cell.TextLabel.Text = shipEvent.preview;
			//cell.TextLabel.SizeToFit();

			cell.DetailTextLabel.AttributedText = GetShipEventDetailText(shipEvent);
			return cell;
		}

		private NSAttributedString GetShipEventDetailText(shipdate shipEvent)
		{
			NSMutableAttributedString detailString = new NSMutableAttributedString();

			detailString.Append(new NSAttributedString(shipEvent.year, new UIStringAttributes
			{
				Font = UIFont.BoldSystemFontOfSize(12)
			}));

			detailString.Append(new NSAttributedString("\t\t\t"));

			detailString.Append(new NSAttributedString(shipEvent.title, new UIStringAttributes
			{
				Font = UIFont.ItalicSystemFontOfSize(12)
			}));

			return detailString;
		}

		/*public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var shipEvent = All[indexPath.Row];
			int i = 0;
		}*/

		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			base.PrepareForSegue(segue, sender);

			if (segue.Identifier == "ShowTimelineDocumentSegue")
			{
				var shipEvent = All[this.TableView.IndexPathForSelectedRow.Row];
				var destination = segue.DestinationViewController as ShipDocumentViewController;
				destination.ShipEvent = shipEvent;
			}

		}
	}
}


