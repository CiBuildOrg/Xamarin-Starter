using System.Collections.Generic;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace App.Template.XForms.Core.ViewModels.Base
{
    [PageViewModel]
    public abstract class BasePageViewModel : MvxViewModel
    {
        private IMvxNavigationService NavigationService { get; }

        protected BasePageViewModel(IMvxNavigationService navigationService)
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