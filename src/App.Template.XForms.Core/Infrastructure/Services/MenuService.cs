using System.Collections.Generic;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Models;
using App.Template.XForms.Core.ViewModels;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace App.Template.XForms.Core.Infrastructure.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMvxNavigationService _navigationService;

        public MenuService(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public List<MenuItem> GetMenuItems()
        {
           return new List<MenuItem>
           {
               new MenuItem
               {
                   Text = "First view",
                   Image = "ic_drawer_settings.png",
                   Command = new MvxCommand(ClearStackAndShowViewModel<FirstViewModel>)
               },

               new MenuItem
               {
                   Text = "Second view",
                   Image = "ic_drawer_about.png",
                   Command = new MvxCommand(ClearStackAndShowViewModel<SecondViewModel>)
               },

               new MenuItem
               {
                   Text = "Third view",
                   Image = "ic_power_settings.png",
                   Command = new MvxCommand(ClearStackAndShowViewModel<ThirdViewModel>)
               }
           };
        }

        private void ClearStackAndShowViewModel<TViewModel>() where TViewModel : IMvxViewModel
        {
            var presentationBundle =
                new MvxBundle(new Dictionary<string, string>
                {
                    {"NavigationMode", "ClearStack"}
                });

            _navigationService.Navigate<TViewModel>(presentationBundle);
        }
    }
}
