using MvvmCross.Plugins.Messenger;

namespace App.Template.XForms.Core.Models.Messages
{
    public class LoginFailureMessage : MvxMessage
    {
        public string ErrorMessage { get; }

        public LoginFailureMessage(object sender, string errorMessage) : base(sender)
        {
            ErrorMessage = errorMessage;
        }
    }
}