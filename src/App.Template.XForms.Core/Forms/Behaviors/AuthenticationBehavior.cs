﻿using App.Template.XForms.Core.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.Template.XForms.Core.Forms.Behaviors
{
    internal class AuthenticationBehaviour : Behavior<View>
    {
        public enum AuthState
        {
            Begin,
            Start,
            Fail,
            Success
        }

        public View TopGroup { get; set; }
        public View FormGroup { get; set; }
        public Label ErrorMessage { get; set; }
        public Button LoginButton { get; set; }
        public ActivityIndicator LoadingIndicator { get; set; }

        private TopGroupBehaviour _topGroupBehaviour;

        protected override async void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);

            _topGroupBehaviour = TopGroup.GetBehaviour<TopGroupBehaviour>();

            var delay = 350;
            await Task.WhenAll(
                _topGroupBehaviour.ViewStart(delay),
                ViewStart(delay)
                );
        }

        private async Task ViewStart(int delay = 350)
        {
            FormGroup.Opacity = 0;
            await Task.Delay(delay);
            await FormGroup.FadeTo(1, 850, Easing.CubicIn);
        }

        public async Task SwitchAuthState(AuthState state)
        {
            LoadingIndicator.IsRunning = false;

            switch (state)
            {
                case AuthState.Begin:
                    await _topGroupBehaviour.SwitchStateToDefault();
                    LoginButton.IsEnabled = true;
                    LoginButton.TextColor = Color.WhiteSmoke;
                    LoginButton.Text = "Log in";
                    break;
                case AuthState.Start:
                    LoginButton.TextColor = Color.WhiteSmoke;
                    LoginButton.Text = "Working...";
                    LoginButton.IsEnabled = false;
                    LoadingIndicator.IsRunning = true;
                    await _topGroupBehaviour.SwitchStateToBusy();
                    break;
                case AuthState.Fail:
                    ErrorMessage.IsVisible = true;
                    await _topGroupBehaviour.SwitchStateToDefault();
                    await InvalidInputAnimation();
                    LoginButton.IsEnabled = true;
                    LoginButton.Text = "Log in";
                    LoginButton.TextColor = Color.WhiteSmoke;
                    break;
                case AuthState.Success:
                    ErrorMessage.IsVisible = false;
                    LoginButton.IsEnabled = false;
                    await Task.WhenAll(
                        _topGroupBehaviour.SwitchStateToSuccess(),
                        FormGroup.FadeTo(0, 400, Easing.SinIn),
                        FormGroup.TranslateTo(0, 1000, 600, Easing.SinIn)
                    );
                    break;
            }
        }

        public async Task SwitchAuthState(AuthState state, string errorMessage)
        {
            ErrorMessage.Text = errorMessage;
            await SwitchAuthState(state);
        }

        private async Task InvalidInputAnimation()
        {
            await ErrorMessage.TranslateTo(15, 0, 75, Easing.SpringIn);
            await ErrorMessage.TranslateTo(-15, 0, 75, Easing.SpringOut);
            await ErrorMessage.TranslateTo(10, 0, 75, Easing.BounceIn);
            await ErrorMessage.TranslateTo(-10, 0, 75, Easing.BounceOut);
            await ErrorMessage.TranslateTo(0, 0, 50, Easing.BounceIn);
        }
    }
}
