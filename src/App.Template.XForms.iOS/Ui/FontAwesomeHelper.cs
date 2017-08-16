using System;
using UIKit;

namespace App.Template.XForms.iOS.Ui
{
    internal class FontAwesomeHelper
    {
        public static UIFont Font(nfloat size)
        {
            return UIFont.FromName("FontAwesome", size);
        }
    }
}