using MvvmCross.Plugins.Messenger;

namespace App.Template.XForms.Core.Utils.Validation
{
    public class ValidationMessage : MvxMessage
    {
        public IErrorCollection Validation { get; private set; }

        public ValidationMessage(IErrorCollection validation, object sender)
            : base(sender)
        {
            Validation = validation;
        }
    }
}