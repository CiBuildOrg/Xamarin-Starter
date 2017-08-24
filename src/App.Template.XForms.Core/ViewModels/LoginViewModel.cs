using MvvmCross.Core.ViewModels;

namespace App.Template.XForms.Core.ViewModels
{
    public class LoginViewModel : MvxViewModel
    {
        public LoginViewModel()
        {
            
        }

    }


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
}
