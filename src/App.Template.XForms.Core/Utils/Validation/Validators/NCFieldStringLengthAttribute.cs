using System;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class NcFieldStringLengthAttribute : NcFieldValidationAttribute
    {
        private NcFieldStringLengthAttribute(string message = null) : base(message) { }

        public NcFieldStringLengthAttribute(int maximumLength, string message = null)
            : base(message)
        {
            MaximumLength = maximumLength;
        }

        public int MaximumLength { get; }

        public int MinimumLength { get; set; }

        public override IValidation CreateValidation(Type valueType)
        {
            if (!valueType.FullName.Contains("MvvmCross.FieldBinding"))
                throw new NotSupportedException("NCFieldStringLength Validator for type " + valueType.Name + " is not supported.");

            return new NcFieldStringLengthValidation(str =>
            {
                if (string.IsNullOrEmpty(str?.Value))
                    return true;

                return str.Value.Length >= MinimumLength && str.Value.Length <= MaximumLength;
            }, MinimumLength, MaximumLength, Message);
        }
    }
}
