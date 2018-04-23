using System;
using CoreFoundation;
using DANFS.Services;
using Foundation;
using UIKit;

namespace DANFS.iOS
{
	public partial class ShipDocumentViewController : UIViewController, IUIWebViewDelegate
	{
		public shipdate ShipEvent { get; internal set; }

		public string LocationGuidToHighlight { get; internal set; }

		public string ShipId { get; set;}

		public ShipDocumentViewController(IntPtr handle) : base(handle)
		{
		}

		private UIWebView ActiveWebView
		{
			get
			{
				if (this.shipDocumentWebView != null)
				{
					return this.shipDocumentWebView;
				}
				return this.shipDocumentWebView2;
			}
		}

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			var dataAccess = TinyIoC.TinyIoCContainer.Current.Resolve<IDataAccess>();

			var shipIDToShow = ShipEvent != null ? ShipEvent.id : ShipId;

			var htmlString = await dataAccess.GetDisplayableHTMLForShip(shipIDToShow);

			ActiveWebView.LoadHtmlString(htmlString, null);

			var shipLookupTable = await dataAccess.GetShipLookupTable();

			this.NavigationItem.Title = shipLookupTable[shipIDToShow].Title;

			//Make sure to hide the toolbar if it was made visible, otherwise layout gets weird.
			this.NavigationController.ToolbarHidden = true;

			this.View.BackgroundColor = UIColor.Green;

		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			this.NavigationController.Toolbar.Hidden = true;

			this.View.SetNeedsLayout();
			this.View.LayoutIfNeeded();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			this.NavigationController.Toolbar.Hidden = false;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		[Export("webViewDidFinishLoad:")]
		public void LoadingFinished(UIWebView webView)
		{
			if (webView.IsLoading) {
				return;
			}

			if (ActiveWebView.EvaluateJavascript("document.readyState") == "complete") {
				// UIWebView object has fully loaded.

				//Wait 2 seconds then execute this on the main loop hoping that hte layout is complete.
				DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(1000000000 * 2) , () =>
				{
					if (ShipEvent != null)
					{
						ActiveWebView.EvaluateJavascript($"document.getElementById('date-{ShipEvent.date_guid}').style.backgroundColor = \"#00FF00\";");
						ActiveWebView.EvaluateJavascript($"document.getElementById('date-{ShipEvent.date_guid}').scrollIntoView(true);");
					}
					if (LocationGuidToHighlight != null)
					{
						ActiveWebView.EvaluateJavascript($"document.getElementById('location-{LocationGuidToHighlight}').style.backgroundColor = \"#00FF00\";");
						ActiveWebView.EvaluateJavascript($"document.getElementById('location-{LocationGuidToHighlight}').scrollIntoView(true);");

						DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(1000000000 * 2), () =>
			   {
				   //Offset up by a height greater than the toolbar height
				   string scrollByDirective = "window.scrollBy(0, -60);";
				   ActiveWebView.EvaluateJavascript(scrollByDirective);
			   });
					}
				});
			}
		}
	}
}


