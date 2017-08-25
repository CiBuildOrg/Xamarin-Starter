using App.Template.XForms.Core.Extensions;
using App.Template.XForms.Core.Forms.Behaviors;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using App.Template.XForms.Core.Models.Messages;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Template.XForms.Core.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView
    {
        private readonly AuthenticationBehaviour _authenticationBehaviour;
        private readonly IMvxMessenger _messenger;

        private MvxSubscriptionToken _loginSuccessSubscriptionToken;
        private MvxSubscriptionToken _loginFailureSubscriptionToken;
        private MvxSubscriptionToken _loginStartSubscriptionToken;

        public LoginView()
        {
            InitializeComponent();
            _authenticationBehaviour = ControllerBag.GetBehaviour<AuthenticationBehaviour>();
            _messenger = Mvx.Resolve<IMvxMessenger>();

            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _loginStartSubscriptionToken =
                _messenger.SubscribeOnThreadPoolThread<StartLoginMessage>(OnStart);

            _loginSuccessSubscriptionToken =
                _messenger.SubscribeOnThreadPoolThread<LoginSuccessMessage>(OnLoginSuccess);

            _loginFailureSubscriptionToken =
                _messenger.SubscribeOnThreadPoolThread<LoginFailureMessage>(OnLoginFailure);
        }

        protected override void OnDisappearing()
        {
            _messenger.Unsubscribe<StartLoginMessage>(_loginStartSubscriptionToken);
            _messenger.Unsubscribe<LoginSuccessMessage>(_loginSuccessSubscriptionToken);
            _messenger.Unsubscribe<LoginFailureMessage>(_loginFailureSubscriptionToken);

            base.OnDisappearing();
        }

        private void OnLoginSuccess(LoginSuccessMessage message)
        {
            SignalSuccess().WaitAsync();
        }

        private void OnLoginFailure(LoginFailureMessage message)
        {
            SignalFailure(message.ErrorMessage).WaitAsync();
        }

        private void OnStart(StartLoginMessage message)
        {
            SignalStart().WaitAsync();
        }

        private async Task SignalSuccess()
        {
            await _authenticationBehaviour.SwitchAuthState(AuthenticationBehaviour.AuthState.Success);
        }

        private async Task SignalFailure(string error)
        {
            await _authenticationBehaviour.SwitchAuthState(AuthenticationBehaviour.AuthState.Fail, error);
        }

        private async Task SignalStart()
        {
            await _authenticationBehaviour.SwitchAuthState(AuthenticationBehaviour.AuthState.Start);
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await _authenticationBehaviour.SwitchAuthState(AuthenticationBehaviour.AuthState.Fail, "User closed");
            if (Navigation.ModalStack.Count > 0)
                await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            // don't allow going back
            return true;
        }
    }
}
