using System;
using DANFS.Services;
using UIKit;

namespace DANFS.iOS
{
	public partial class ShipDocumentViewController : UIViewController
	{
		public shipdate ShipEvent { get; internal set; }

		public ShipDocumentViewController(IntPtr handle) : base(handle)
		{
		}

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			var dataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess>();

			var htmlString = await dataAccess.GetDisplayableHTMLForShip(ShipEvent.name);

			shipDocumentWebView.LoadHtmlString(htmlString, null);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


