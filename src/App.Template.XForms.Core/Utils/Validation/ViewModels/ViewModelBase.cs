using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace App.Template.XForms.Core.Utils.Validation.ViewModels
{
    public class ViewModelBase : MvxViewModel
    {
        protected IMvxMessenger Messenger { get; }

        public ViewModelBase(IMvxMessenger messenger)
        {
            Messenger = messenger;
        }
    }
}