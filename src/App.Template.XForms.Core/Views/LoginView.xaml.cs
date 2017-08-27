using App.Template.XForms.Core.Extensions;
using App.Template.XForms.Core.Forms.Behaviors;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using App.Template.XForms.Core.Models.Messages;
using App.Template.XForms.Core.ViewModels;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
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
                _messenger.SubscribeOnMainThread<StartLoginMessage>(async x => await SignalStart());

            _loginSuccessSubscriptionToken =
                _messenger.SubscribeOnMainThread<LoginSuccessMessage>(async x => await SignalSuccess(x.ActionToExecute));

            _loginFailureSubscriptionToken =
                _messenger.SubscribeOnMainThread<LoginFailureMessage>(async x => await SignalFailure(x.ErrorMessage));
        }

        protected override void OnDisappearing()
        {
            _messenger.Unsubscribe<StartLoginMessage>(_loginStartSubscriptionToken);
            _messenger.Unsubscribe<LoginSuccessMessage>(_loginSuccessSubscriptionToken);
            _messenger.Unsubscribe<LoginFailureMessage>(_loginFailureSubscriptionToken);

            base.OnDisappearing();
        }

        private async Task SignalSuccess(Action after)
        {
            await _authenticationBehaviour.SwitchAuthState(AuthenticationBehaviour.AuthState.Success);
            after();
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
