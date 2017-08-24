using System;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public abstract class NcFieldValidationAttribute : ValidationAttribute
    {
        protected NcFieldValidationAttribute(string message) : base(message)
        {
            
        }
    }
}