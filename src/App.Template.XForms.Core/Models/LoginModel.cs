using App.Template.XForms.Core.Utils.Validation;
using FluentValidation;

namespace App.Template.XForms.Core.Models
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            // rules here
            RuleFor(x => x.UserName).Must(x => !string.IsNullOrEmpty(x)).WithMessage("Username must not be empty");
            RuleFor(x => x.Password).Must(x => !string.IsNullOrEmpty(x)).WithMessage("Password must not be empty");
            RuleFor(x => x.UserName).Must(x => x.Length >= 3).WithMessage("Username invalid (more than 3 chars)");
            RuleFor(x => x.Password).Must(x => x.Length >= 3).WithMessage("Password invalid (more than 3 chars)");
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
