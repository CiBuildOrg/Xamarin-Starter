using System;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class ShouldBeLongAttribute : ValidationAttribute
    {
        public ShouldBeLongAttribute(string message = null)
            : base(message)
        {
        }

        public override IValidation CreateValidation(Type valueType)
        {
            return new ShouldBeLongValidation(Message);
        }
    }
}