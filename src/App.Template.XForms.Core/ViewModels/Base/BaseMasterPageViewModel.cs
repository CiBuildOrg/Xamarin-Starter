using System.Collections.Generic;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.ViewModels;

namespace App.Template.XForms.Core.ViewModels.Base
{
    public abstract class BaseMasterPageViewModel<T> : MvxMasterDetailViewModel<T> where T : IMvxViewModel
    {
        private IMvxNavigationService NavigationService { get; }

        protected BaseMasterPageViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        protected void ClearStackAndShowViewModel<TViewModel>()
            where TViewModel : IMvxViewModel
        {
            var presentationBundle =
                new MvxBundle(new Dictionary<string, string>
                {
                    {"NavigationMode", "ClearStack"}
                });

            NavigationService.Navigate<TViewModel>(presentationBundle);
        }
    }
}