// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DANFS.iOS
{
	[Register ("ShipDocumentViewController")]
	partial class ShipDocumentViewController
	{
		[Outlet]
		UIKit.UIWebView shipDocumentWebView { get; set; }

		[Outlet]
		UIKit.UIWebView shipDocumentWebView2 { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (shipDocumentWebView != null) {
				shipDocumentWebView.Dispose ();
				shipDocumentWebView = null;
			}

			if (shipDocumentWebView2 != null) {
				shipDocumentWebView2.Dispose ();
				shipDocumentWebView2 = null;
			}
		}
	}
}
