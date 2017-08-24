using System;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class RegexAttribute : ValidationAttribute
    {
        private readonly string _regex;

        public RegexAttribute(string regex, string message)
            : base(message)
        {
            _regex = regex;
        }

        public override IValidation CreateValidation(Type valueType)
        {
            return new RegexValidation(_regex, Message);
        }
    }
}