﻿using System;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Models;
using FluentValidation;

namespace App.Template.XForms.Core.CustomValidators
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator(IAppSettings appSetting)
        {

            if (appSetting == null)
                throw new Exception("wtf");

            // rules here
            RuleFor(x => x.UserName).Must(x => !string.IsNullOrEmpty(x))
                .WithMessage("Username must not be empty");
            RuleFor(x => x.Password).Must(x => !string.IsNullOrEmpty(x))
                .WithMessage("Password must not be empty");

            RuleFor(x => x.UserName).Must(x => x.Length >= appSetting.Validation.UsernameMinLength)
                .When(x => !string.IsNullOrEmpty(x.UserName))
                .WithMessage($"Username invalid (more than {appSetting.Validation.UsernameMinLength} chars)");
            RuleFor(x => x.Password).Must(x => x.Length >= appSetting.Validation.PasswordMinLength)
                .When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage($"Password invalid (more than {appSetting.Validation.PasswordMinLength} chars)");
        }
    }
}