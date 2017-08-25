using System;
using System.Threading.Tasks;
using App.Template.XForms.Core.Models;
using App.Template.XForms.Core.Resources;
using Autofac.Core;
using MvvmCross.Core.ViewModels;

namespace App.Template.XForms.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        public LoginViewModel()
        {
            LoginConfig = LoadConfiguration();
            LoginModel = new LoginModel();
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

        private LoginModel _loginModel;

        public LoginModel LoginModel
        {
            get => _loginModel;
            set => SetProperty(ref _loginModel, value, nameof(LoginModel));
        }

        public bool CanContinue()
        {
            var loginModel = new LoginModel();

            var errors = loginModel.ValidateModel();

            return false;
        }

        private IMvxCommand _submitCommand;

        public IMvxCommand SubmitCommand => _submitCommand ??
                                            (_submitCommand = new MvxAsyncCommand(async () => await Submit())); 



        public async Task Submit()
        {
            await Task.Delay(2000);

            var validation = LoginModel.ValidateModel();
            if (validation.Success)
            {

            }
            else
            {
                
            }
        }
    }
}
