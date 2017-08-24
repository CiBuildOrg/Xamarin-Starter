using App.Template.XForms.Core.Extensions;
using App.Template.XForms.Core.Forms.Behaviors;
using System;
using System.Threading;
using App.Template.XForms.Core.Models;
using App.Template.XForms.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace App.Template.XForms.Core.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView
    {
        private AuthenticationBehaviour _authenticationBehaviour;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public LoginView()
        {
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
