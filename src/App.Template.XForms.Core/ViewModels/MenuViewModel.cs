using App.Template.XForms.Core.Models;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.ViewModels.Base;

namespace App.Template.XForms.Core.ViewModels
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class MenuViewModel : BaseMasterPageViewModel<HomeViewModel>
    {
        #region Fields

        private IEnumerable<MenuItem> _menu;
        private MenuItem _selectedMenuItem;
        private MvxCommand<MenuItem> _onSelectedMenuItemChangedCommand;
        private string _userFullName;
        private string _userEmail;

        #endregion Fields

        #region Properties

        public IEnumerable<MenuItem> Menu
        {
            get => _menu;
            set => SetProperty(ref _menu, value);
        }

        public MenuItem SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                SetProperty(ref _selectedMenuItem, value);
                if (value != null)
                {
                    OnSelectedMenuItemChangedCommand.Execute(value);
                }
            }
        }

        public string UserFullName
        {
            get => _userFullName;
            set => SetProperty(ref _userFullName, value);
        }

        public string UserEmail
        {
            get => _userEmail;
            set => SetProperty(ref _userEmail, value);
        }

        private ICommand OnSelectedMenuItemChangedCommand
        {
            get
            {
                return _onSelectedMenuItemChangedCommand ?? (_onSelectedMenuItemChangedCommand =
                           new MvxCommand<MenuItem>(item =>
                           {
                               if (item == null)
                                   return;

                               item.Command.Execute();
                           }));
            }
        }

        #endregion Properties

        public MenuViewModel(IMvxNavigationService navigationService, IMenuService menuService) : base(navigationService)
        {
            UserFullName = "Adam";
            UserEmail = "adam@noname.com";
            Menu = menuService.GetMenuItems();
        }

        public override void RootContentPageActivated()
        {
            SelectedMenuItem = null;
        }
    }
}