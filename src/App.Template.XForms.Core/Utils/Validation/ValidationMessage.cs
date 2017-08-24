using MvvmCross.Plugins.Messenger;
using App.Template.XForms.Core.Contracts;

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