using App.Template.XForms.Core.Models;
using App.Template.XForms.Core.Resources;
using MvvmCross.Core.ViewModels;

namespace App.Template.XForms.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        public LoginViewModel()
        {
            LoginConfig = LoadConfiguration();
        }

        private static AuthPageConfiguration LoadConfiguration()
        {
            return new AuthPageConfiguration
            {
                Title = Texts.AuthPageTitle,
                SubTitle = Texts.AuthPageSubtitle,
                ShowCloseButton = false,
                ShowHeader = false,
                ShowRegistrationButton = false
            };
        }

        private AuthPageConfiguration _loginConfig;

        public AuthPageConfiguration LoginConfig
        {
            get => _loginConfig;
            set => SetProperty(ref _loginConfig, value);
        }

    }
}
