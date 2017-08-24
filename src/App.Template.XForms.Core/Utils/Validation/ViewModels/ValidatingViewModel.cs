using System;
using MvvmCross.Plugins.Messenger;

namespace App.Template.XForms.Core.Utils.Validation.ViewModels
{
    public abstract class ValidatingViewModel : ViewModelBase
    {
        public IValidator Validator { get; }

        protected ValidatingViewModel(IValidator validator, IMvxMessenger messenger)
            : base(messenger)
        {
            Validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public IErrorCollection Validate(string group = null)
        {
            return Validator.Validate(this, group);
        }

        public bool ValidateAndSendMessage(string group = null)
        {
            var result = Validate(group);
            if (!result.IsValid)
            {
                Messenger.Publish(new ValidationMessage(result, this));
            }
            return result.IsValid;
        }

        public bool IsValid(string group = null)
        {
            return Validate(group).IsValid;
        }
    }
}