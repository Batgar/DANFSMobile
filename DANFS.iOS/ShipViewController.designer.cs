// WARNING
//
// This file has been generated automatically by Xamarin Studio Business to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DANFS.iOS
{
	[Register ("ShipViewController")]
	partial class ShipViewController
	{
		[Outlet]
		MapKit.MKMapView locationMapView { get; set; }

		[Outlet]
		UIKit.UITextView mainTextView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (mainTextView != null) {
				mainTextView.Dispose ();
				mainTextView = null;
			}

			if (locationMapView != null) {
				locationMapView.Dispose ();
				locationMapView = null;
			}
		}
	}
}