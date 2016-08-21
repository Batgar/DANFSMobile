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
	[Register ("ShipViewController")]
	partial class ShipViewController
	{

		[Outlet]
		UIKit.UIView locationsContainerView { get; set; }


		[Outlet]
		UIKit.UIView mapContainerView { get; set; }

		[Action ("showComponent:")]
		partial void showComponent (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (locationsContainerView != null) {
				locationsContainerView.Dispose ();
				locationsContainerView = null;
			}

			if (mapContainerView != null) {
				mapContainerView.Dispose ();
				mapContainerView = null;
			}
		}
	}
}
