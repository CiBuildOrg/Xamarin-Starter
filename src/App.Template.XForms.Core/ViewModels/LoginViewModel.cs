using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using App.Template.XForms.Core.Models;
using App.Template.XForms.Core.Models.Messages;
using App.Template.XForms.Core.Resources;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace App.Template.XForms.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        private readonly IMvxMessenger _messenger;

        public LoginViewModel(IMvxMessenger messenger)
        {
            _messenger = messenger;
            LoginConfig = LoadConfiguration();
            LoginModel = new LoginModel();
        }

        private static AuthPageConfiguration LoadConfiguration()
        {
            return new AuthPageConfiguration
            {
                SubTitle = Texts.AuthPageSubtitle,
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

        private IMvxCommand _submitCommand;
        public IMvxCommand SubmitCommand => _submitCommand ??
                                            (_submitCommand = new MvxAsyncCommand(async () => await Submit())); 

        public async Task Submit()
        {
            _messenger.Publish(new StartLoginMessage(this));

            var validation = LoginModel.ValidateModel();
            if (validation.Success)
            {
                await Task.Delay(6000);
                // send message

                _messenger.Publish(new LoginSuccessMessage(this, NavigateAway));
            }
            else
            {
                _messenger.Publish(new LoginFailureMessage(this, validation.Failures.First().ErrorMessage));
            }
        }
        private static void ClearStackAndShowViewModel<TViewModel>() where TViewModel : IMvxViewModel
        {
            var presentationBundle =
                new MvxBundle(new Dictionary<string, string>
                {
                    {"NavigationMode", "ClearStack"}
                });

            Mvx.Resolve<IMvxNavigationService>().Navigate<TViewModel>(presentationBundle);
        }

        [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public void NavigateAway()
        {
            ClearStackAndShowViewModel<MenuViewModel>();
        }
    }
}
