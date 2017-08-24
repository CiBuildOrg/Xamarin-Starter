using System;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class NcFieldShouldBeLongAttribute : NcFieldValidationAttribute
    {
        public NcFieldShouldBeLongAttribute(string message = null)
            : base(message)
        {
        }

        public override IValidation CreateValidation(Type valueType)
        {
            return new NcFieldShouldBeLongValidation(Message);
        }
    }
}