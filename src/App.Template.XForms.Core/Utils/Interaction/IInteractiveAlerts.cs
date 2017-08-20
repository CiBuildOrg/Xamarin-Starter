using System;

namespace App.Template.XForms.Core.Utils.Interaction
{
    public interface IInteractiveAlerts
    {
        IDisposable ShowAlert(InteractiveAlertConfig alertConfig);

        IDisposable ShowAlert(EditableInteractiveAlertConfig alertConfig);
    }
}