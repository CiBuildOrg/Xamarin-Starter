using System;
using Android.Runtime;
using App.Template.XForms.Core.Utils.Interaction;

namespace App.Template.XForms.Android.Infrastructure.Interaction.Alerts
{
    public class InteractiveDialogFragment : BaseInteractiveDialogFragment<InteractiveAlertConfig>
    {
        public InteractiveDialogFragment()
        {

        }

        protected InteractiveDialogFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }
    }
}