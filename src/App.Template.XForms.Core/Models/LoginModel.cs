using App.Template.XForms.Core.Utils.Validation;

namespace App.Template.XForms.Core.Models
{
    public class LoginModel : ValidateableModelBase<LoginModel>
    {
        private string _password;
        private string _userName;

        [Validateable]
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value, nameof(UserName));
        }

        [Validateable]
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value, nameof(Password));
        }
    }
}
