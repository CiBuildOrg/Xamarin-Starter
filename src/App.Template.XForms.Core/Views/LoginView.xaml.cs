using App.Template.XForms.Core.Extensions;
using App.Template.XForms.Core.Forms.Behaviors;
using System;
using System.Threading;
using Xamarin.Forms.Xaml;

namespace App.Template.XForms.Core.Views
{
    /// <summary>
    /// Configure the appereance of the authentication page.
    /// </summary>
    public class AuthPageConfiguration
    {
        public string Title { get; set; } = "Authenticate";
        public string SubTitle { get; set; } = "Sign in to your Account";
        public bool ShowCloseButton { get; set; } = false;
        public bool ShowRegistrationButton { get; set; } = false;
        public bool ShowHeader { get; set; } = false;
        public uint MinUsernameLength { get; set; } = 2;
        public uint MinPasswordLength { get; set; } = 4;
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView
    {
        AuthenticationBehaviour _authenticationBehaviour;
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        AuthPageConfiguration _authPageConfiguration;

        public LoginView()
        {
            _authPageConfiguration = new AuthPageConfiguration();
            //_authenticator = authenticator;
            //_authenticator.Completed += AuthenticatorOnCompleted;
            //_authenticator.Error += AuthenticatorOnError;

            InitializeComponent();
            //AuthFieldsToEntries();
            ConfigurePage();

            _authenticationBehaviour = ControllerBag.GetBehaviour<AuthenticationBehaviour>();
        }

        protected void ConfigurePage()
        {
            HeaderTitle.Text = _authPageConfiguration.Title;
            HeaderMessage.Text = _authPageConfiguration.SubTitle;
            CloseIcon.IsVisible = _authPageConfiguration.ShowCloseButton;
            HeaderTitle.IsVisible = _authPageConfiguration.ShowHeader;
            RegistrationButton.IsVisible = _authPageConfiguration.ShowRegistrationButton;
        }

        protected override void OnDisappearing()
        {
            _cancellationTokenSource.Cancel();
            base.OnDisappearing();
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            await _authenticationBehaviour.SwitchAuthState(AuthenticationBehaviour.AuthState.Start);
            
            //foreach (var pair in _fieldToEntryPairs)
            //    pair.Key.Value = pair.Value.Text;

            //if (!await ValidateEntries())
            //    return;

            try
            {
                //var account = await _authenticator.SignInAsync(_cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {

                await _authenticationBehaviour.SwitchAuthState(AuthenticationBehaviour.AuthState.Fail, ex.Message);
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await _authenticationBehaviour.SwitchAuthState(AuthenticationBehaviour.AuthState.Fail, "User closed");
            if (Navigation.ModalStack.Count > 0)
                await Navigation.PopModalAsync();
        }

        //private async Task<bool> ValidateEntries()
        //{
        //    foreach (var entryValidator in _entryValidators)
        //    {
        //        var validation = entryValidator.Validate();

        //        if (!validation.Item1)
        //        {
        //            await _authenticationBehaviour.SwitchAuthState(AuthenticationBehaviour.AuthState.Fail, validation.Item2);
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        //private async void AuthenticatorOnError(object sender, AuthenticatorErrorEventArgs eventArgs)
        //{
        //    await _authenticationBehaviour.SwitchAuthState(AuthenticationBehaviour.AuthState.Fail, eventArgs.Message);
        //}

        //private async void AuthenticatorOnCompleted(object sender, AuthenticatorCompletedEventArgs eventArgs)
        //{
        //    if (eventArgs.IsAuthenticated)
        //    {
        //        eventArgs.Account.Properties.Add("RememberMe", InputRememberMe.IsToggled.ToString());
        //        await _authenticationBehaviour.SwitchAuthState(AuthenticationBehaviour.AuthState.Success);
        //        await Task.Delay(1500);

        //        await Navigation.PopModalAsync();
        //    }
        //}

        protected override bool OnBackButtonPressed()
        {
            OnCancelClicked(this, EventArgs.Empty);
            return base.OnBackButtonPressed();
        }
    }
}
