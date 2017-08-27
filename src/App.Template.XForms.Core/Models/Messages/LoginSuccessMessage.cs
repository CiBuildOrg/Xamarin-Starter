using System;
using MvvmCross.Plugins.Messenger;

namespace App.Template.XForms.Core.Models.Messages
{
    public class LoginSuccessMessage : MvxMessage
    {
        public Action ActionToExecute { get; }
        public LoginSuccessMessage(object sender, Action actionToExecute) : base(sender)
        {
            ActionToExecute = actionToExecute;
        }
    }
}
