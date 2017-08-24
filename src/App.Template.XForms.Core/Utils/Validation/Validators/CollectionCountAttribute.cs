using System;
using System.Diagnostics.CodeAnalysis;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class CollectionCountAttribute : ValidationAttribute
    {
        public CollectionCountAttribute(string message = null) 
            : base(message)
        {
        }

        private CollectionCountAttribute(Type type, string minimum, string maximum, string message = null)
            : base(message)
        {
            Maximum = maximum;
            Minimum = minimum;
        }

        public CollectionCountAttribute(int minimum, int maximum, string message = null) 
            : this(typeof(int), minimum.ToString(), maximum.ToString(), message)
        {
        }

        public object Maximum { get; }

        public object Minimum { get; }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        public Type OperandType { get; private set; }

        public override IValidation CreateValidation(Type valueType)
        {
            if (!valueType.FullName.Contains("System.Collections") && !valueType.IsArray)
                throw new NotSupportedException("CapacityAttribute Validator for type " + valueType.Name + " is not supported.");

            return new CollectionCountValidation(num => num >= int.Parse(Minimum.ToString()) && num <= int.Parse(Maximum.ToString()),
                Minimum, Maximum, Message);
        }
    }
}
