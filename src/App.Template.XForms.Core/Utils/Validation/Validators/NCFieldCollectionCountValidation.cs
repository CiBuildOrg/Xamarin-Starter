using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class NcFieldCollectionCountValidation : IValidation
    {
        private readonly Func<int, bool> _predicate;
        private readonly string _message;
        private readonly object _maximum;
        private readonly object _minimum;

        public NcFieldCollectionCountValidation(Func<int, bool> predicate, object minimum, object maximum, string message)
        {
            _predicate = predicate;
            _minimum = minimum;
            _maximum = maximum;
            _message = message;
        }

        public IErrorInfo Validate(string fieldName, object value, object subject)
        {
            if (value == null)
                return null;

            var incValue = value.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == "Value").GetValue(value);
            if (incValue == null)
                return null;

            var incValueType = incValue.GetType();
            if (!incValueType.FullName.Contains("System.Collections") && !incValueType.IsArray)
                throw new NotSupportedException("NCFieldCollectionCountAtribute Validator for type " + value.GetType().FullName + " is not supported.");

            return Validate(fieldName, incValue as ICollection, subject);
        }

        public IErrorInfo Validate(string fieldName, ICollection value, object subject)
        {
            if (!_predicate(value?.Count ?? 0))
            {
                return new ErrorInfo(fieldName, _message == null ?
                    string.Format("The Count of {0} must between {1} and {2}", fieldName, _minimum, _maximum) :
                    string.Format(_message, fieldName, _minimum, _maximum)
                    );
            }
            return null;
        }
    }
}
