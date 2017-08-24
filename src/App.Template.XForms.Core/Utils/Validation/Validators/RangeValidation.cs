using System;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class RangeValidation : IValidation
    {
        private readonly Func<decimal, bool> _predicate;
        private readonly string _message;
        private readonly object _maximum;
        private readonly object _minimum;

        public RangeValidation(Func<decimal, bool> predicate, object minimum, object maximum, string message)
        {
            _predicate = predicate;
            _minimum = minimum;
            _maximum = maximum;
            _message = message;
        }

        public IErrorInfo Validate(string propertyName, object value, object subject)
        {
            return Validate(propertyName, decimal.Parse(value.ToString()), subject);
        }

        public IErrorInfo Validate(string propertyName, decimal value, object subject)
        {
            if (!_predicate(value))
            {
                return new ErrorInfo(propertyName, _message == null ? 
                    string.Format("The Range of {0} must between {1} and {2}", propertyName, _minimum, _maximum) : 
                    string.Format(_message, propertyName, _minimum, _maximum)
                    );
            }
            return null;
        }
    }
}