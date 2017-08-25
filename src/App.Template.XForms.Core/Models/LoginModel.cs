using System.ComponentModel;
using App.Template.XForms.Core.Utils.Validation;
using FluentValidation;

namespace App.Template.XForms.Core.Models
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            // rules here
        }
    }

    public class LoginModel : ValidateableModelBase<LoginModel, LoginModelValidator>
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
