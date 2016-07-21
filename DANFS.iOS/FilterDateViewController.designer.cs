// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace DANFS.iOS
{
    [Register ("FilterDateViewController")]
    partial class FilterDateViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIDatePicker datePicker { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField dayRange { get; set; }

        [Action ("DayRangeValueChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DayRangeValueChanged (UIKit.UITextField sender);

        [Action ("DatePickerValueChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DatePickerValueChanged (UIKit.UIDatePicker sender);

        [Action ("DayRangeValueEditingChanged:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DayRangeValueEditingChanged (UIKit.UITextField sender);

        [Action ("ClearFilterTouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ClearFilterTouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (datePicker != null) {
                datePicker.Dispose ();
                datePicker = null;
            }

            if (dayRange != null) {
                dayRange.Dispose ();
                dayRange = null;
            }
        }
    }
}