using System;

// ReSharper disable RedundantAssignment

namespace App.Template.XForms.iOS.Infrastructure.Interaction
{
    // The Main Class
    // https://github.com/vikmeup/SCLAlertView-Swift/blob/master/SCLAlertView/SCLAlertView.swift#L309
    // https://github.com/dogo/SCLAlertView

    // Allow alerts to be closed/renamed in a chainable manner
    // Example: SCLAlertView().showSuccess(self, title: "Test", subTitle: "Value").close()
    public class SclAlertViewResponder
    {
        // Initialisation and Title/Subtitle/Close functions
        public SclAlertViewResponder(InteractiveAlertView alertview)
        {
            Alertview = alertview;
        }

        protected InteractiveAlertView Alertview { get; }

        public void SetTitle(string title)
        {
            Alertview.SetTitle(title);
        }

        public void SetSubTitle(string subTitle)
        {
            Alertview.SetSubTitle(subTitle);
        }

        public void Close()
        {
            Alertview.HideView();
        }

        public void SetDismissBlock(Action dismissBlock)
        {
            Alertview.SetDismissBlock(dismissBlock);
        }
    }
}