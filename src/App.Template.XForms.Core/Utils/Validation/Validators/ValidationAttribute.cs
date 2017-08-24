using System;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Property, AllowMultiple = true)]
    public abstract class ValidationAttribute : Attribute
    {
        public string Message { get; }
        public string[] Groups { get; set; }

        protected ValidationAttribute(string message)
        {
            Message = message;
        }

        public abstract IValidation CreateValidation(Type valueType);
    }
}