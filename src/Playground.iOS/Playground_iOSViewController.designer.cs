// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Playground.iOS
{
    [Register ("Playground_iOSViewController")]
    partial class Playground_iOSViewController
    {
        [Outlet]
        UIKit.UIProgressView progress { get; set; }

        [Outlet]
        UIKit.UILabel md5sum { get; set; }

        [Outlet]
        UIKit.UITextView result { get; set; }

        [Action ("cancelIt:")]
        partial void cancelIt (Foundation.NSObject sender);

        [Action ("doIt:")]
        partial void doIt (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (md5sum != null) {
                md5sum.Dispose ();
                md5sum = null;
            }

            if (progress != null) {
                progress.Dispose ();
                progress = null;
            }

            if (result != null) {
                result.Dispose ();
                result = null;
            }
        }
    }
}