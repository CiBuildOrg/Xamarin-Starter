using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Models;
using App.Template.XForms.Core.Models.Messages;
using App.Template.XForms.Core.Resources;
using App.Template.XForms.Core.ViewModels.Base;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;


namespace App.Template.XForms.Core.ViewModels
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class LoginViewModel : BasePageViewModel
    {
        private readonly IMvxMessenger _messenger;
        private readonly IAuthenticationService _authenticationService;

        public LoginViewModel(IMvxMessenger messenger, IMvxNavigationService navigationService, IAuthenticationService authenticationService)
            : base(navigationService)
        {
            _messenger = messenger;
            _authenticationService = authenticationService;
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

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public AuthPageConfiguration LoginConfig
        {
            get => _loginConfig;
            set => SetProperty(ref _loginConfig, value);
        }

        private LoginModel _loginModel;

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public LoginModel LoginModel
        {
            get => _loginModel;
            set => SetProperty(ref _loginModel, value, nameof(LoginModel));
        }

        private IMvxCommand _submitCommand;
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public IMvxCommand SubmitCommand => _submitCommand ??
                                            (_submitCommand = new MvxAsyncCommand(async () => await Submit()));

        private async Task Submit()
        {
            _messenger.Publish(new StartLoginMessage(this));

            var validation = LoginModel.ValidateModel();
            if (validation.Success)
            {
                await Task.Delay(6000);
                // send message

                var tokenResponse = await _authenticationService.GetAccessToken(LoginModel.UserName, LoginModel.Password, CancellationToken.None);

                if (!tokenResponse.HasAccessToken)
                {
                    _messenger.Publish(new LoginFailureMessage(this, tokenResponse.Error.ErrorDescription));
                }
                else
                {
                    _messenger.Publish(new LoginSuccessMessage(this, NavigateAway));
                }
            }
            else
            {
                _messenger.Publish(new LoginFailureMessage(this, validation.Failures.First().ErrorMessage));
            }
        }

        [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public void NavigateAway()
        {
            ClearStackAndShowViewModel<MenuViewModel>();
        }
    }
}
