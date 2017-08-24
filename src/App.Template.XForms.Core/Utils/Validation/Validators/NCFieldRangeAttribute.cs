using System;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class NcFieldRangeAttribute : NcFieldValidationAttribute
    {
        private NcFieldRangeAttribute(string message = null) 
            : base(message)
        {
        }

        private NcFieldRangeAttribute(string minimum, string maximum, string message = null)
            : base(message)
        {
            Maximum = maximum;
            Minimum = minimum;
        }

        public NcFieldRangeAttribute(double minimum, double maximum, string message = null) 
            : this(minimum.ToString(), maximum.ToString(), message)
        {
        }

        public NcFieldRangeAttribute(int minimum, int maximum, string message = null) 
            : this(minimum.ToString(), maximum.ToString(), message)
        {
        }

        public object Maximum { get; }

        public object Minimum { get; }

        public override IValidation CreateValidation(Type valueType)
        {
            if (!valueType.FullName.Contains("MvvmCross.FieldBinding"))
                throw new NotSupportedException("NCFieldRange Validator for type " + valueType.Name + " is not supported.");

            return new NcFieldRangeValidation(num => num.Value >= decimal.Parse(Minimum.ToString()) && num.Value <= decimal.Parse(Maximum.ToString()), 
                Minimum, Maximum, Message);
        }
    }
}
