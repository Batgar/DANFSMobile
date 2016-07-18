using System;
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
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		[Export("webViewDidFinishLoad:")]
		public void LoadingFinished(UIWebView webView)
		{
			if (ShipEvent != null)
			{
				ActiveWebView.EvaluateJavascript($"document.getElementById('date-{ShipEvent.date_guid}').scrollIntoView()");
				ActiveWebView.EvaluateJavascript($"document.getElementById('date-{ShipEvent.date_guid}').style.backgroundColor = \"#00FF00\"");
			}
			if (LocationGuidToHighlight != null)
			{
				ActiveWebView.EvaluateJavascript($"document.getElementById('location-{LocationGuidToHighlight}').scrollIntoView()");
				ActiveWebView.EvaluateJavascript($"document.getElementById('location-{LocationGuidToHighlight}').style.backgroundColor = \"#00FF00\"");
			}
		}
	}
}


