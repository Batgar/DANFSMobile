using System;
using DANFS.Services;
using UIKit;
using System.Collections.Generic;
using System.Linq;

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

			cell.TextLabel.Lines = 2;
			cell.TextLabel.AdjustsFontSizeToFitWidth = true;
			cell.TextLabel.AdjustsLetterSpacingToFitWidth = true;
			cell.TextLabel.MinimumFontSize = 4;

				//DataAccess.GetTodayInNavyHistoryByYear(Sections.ElementAt((int)indexPath.Section)).ElementAt(indexPath.Row);
			cell.TextLabel.Text = shipEvent.preview;
			//cell.TextLabel.SizeToFit();

			cell.DetailTextLabel.Text = shipEvent.name + " " + shipEvent.year;
			return cell;
		}
	}
}


