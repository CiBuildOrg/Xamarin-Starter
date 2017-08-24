using System;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class NcFieldCollectionCountAttribute : NcFieldValidationAttribute
    {
        private NcFieldCollectionCountAttribute(string message = null) 
            : base(message)
        {
        }

        private NcFieldCollectionCountAttribute(string minimum, string maximum, string message = null)
            : base(message)
        {
            Maximum = maximum;
            Minimum = minimum;
        }

        public NcFieldCollectionCountAttribute(int minimum, int maximum, string message = null) 
            : this(minimum.ToString(), maximum.ToString(), message)
        {
        }

        public object Maximum { get; }

        public object Minimum { get; }

        public override IValidation CreateValidation(Type valueType)
        {
            if (!valueType.FullName.Contains("MvvmCross.FieldBinding"))
                throw new NotSupportedException("NCFieldCollectionCountAtribute Validator for type " + valueType.Name + " is not supported.");

            return new NcFieldCollectionCountValidation(num => num >= int.Parse(Minimum.ToString()) && num <= int.Parse(Maximum.ToString()),
                Minimum, Maximum, Message);
        }
    }
}
