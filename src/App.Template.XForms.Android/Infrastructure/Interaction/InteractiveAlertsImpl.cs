using System;
using Android.Support.V7.App;
using App.Template.XForms.Android.Infrastructure.Interaction.Alerts;
using App.Template.XForms.Core.Utils.Interaction;

namespace App.Template.XForms.Android.Infrastructure.Interaction
{
    public class InteractiveAlertsImpl : IInteractiveAlerts
    {
        private const string DefaultDialogTag = "InteractiveAlert";

        protected internal Func<AppCompatActivity> TopActivityFunc { get; set; }

        public InteractiveAlertsImpl(Func<AppCompatActivity> topActivityFunc)
        {
            TopActivityFunc = topActivityFunc;
        }

        public IDisposable ShowAlert(EditableInteractiveAlertConfig alertConfig)
        {
            var activity = TopActivityFunc();
            var dialogAlert = EditableInteractiveDialogFragment.NewInstance<EditableInteractiveDialogFragment>(alertConfig);
            dialogAlert.Show(activity.SupportFragmentManager, DefaultDialogTag);
            return new DisposableAction(dialogAlert.Dismiss);
        }

        public IDisposable ShowAlert(InteractiveAlertConfig alertConfig)
        {
            var activity = TopActivityFunc();
            var dialogAlert = InteractiveDialogFragment.NewInstance<InteractiveDialogFragment>(alertConfig);
            dialogAlert.Show(activity.SupportFragmentManager, DefaultDialogTag);
            return new DisposableAction(dialogAlert.Dismiss);
        }
    }
}