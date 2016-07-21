using System;
using Foundation;
using UIKit;

namespace DANFS.iOS
{
	public partial class FilterDateViewController : UIViewController
	{
		public FilterDateViewController(IntPtr handle) : base(handle)
		{
			this.ClearFilter = false;
		}

		partial void ClearFilterTouchUpInside(UIButton sender)
		{
			this.ClearFilter = true;

			this.PerformSegue("DismissDialog", null);
		}

		public bool ClearFilter { get; set;}

		partial void DayRangeValueEditingChanged(UITextField sender)
		{
			int parsedDayRange = 0;
			if (Int32.TryParse(sender.Text, out parsedDayRange))
			{
				this.DayRange = parsedDayRange;
			}
		}

		partial void DatePickerValueChanged(UIDatePicker sender)
		{
			this.ChosenDateTime = sender.Date.NSDateToDateTime().Date;
		}

		partial void DayRangeValueChanged(UITextField sender)
		{
			
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			this.datePicker.Date = ChosenDateTime.DateTimeToNSDate();
			this.dayRange.Text = Convert.ToString(this.DayRange);
		}

		public DateTime ChosenDateTime
		{
			get; set;
		}

		public int DayRange
		{
			get; set;
		}



		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}


	}
}


