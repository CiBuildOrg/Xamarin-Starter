using System;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class NcFieldRegexAttribute : NcFieldValidationAttribute
    {
        private readonly string _regex;

        public NcFieldRegexAttribute(string regex, string message)
            : base(message)
        {
            _regex = regex;
        }

        public override IValidation CreateValidation(Type valueType)
        {
            return new NcFieldRegexValidation(_regex, Message);
        }
    }
}