using System;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class StringLengthValidation : IValidation
    {
        private readonly Func<string, bool> _predicate;
        private readonly string _message;
        private readonly int _maximumLength;
        private readonly int _minimumLength;

        public StringLengthValidation(Func<string, bool> predicate, int minimumLength, int maximumLength, string message)
        {
            _predicate = predicate;
            _maximumLength = maximumLength;
            _minimumLength = minimumLength;
            _message = message;
        }

        public IErrorInfo Validate(string propertyName, object value, object subject)
        {
            if (value == null)
                return null;
            return Validate(propertyName, value.ToString(), subject);
        }

        public IErrorInfo Validate(string propertyName, string value, object subject)
        {
            if (!_predicate(value))
            {
                return new ErrorInfo(propertyName, _message == null ? 
                    string.Format("The Length of {0} must between {1} and {2}", propertyName, _minimumLength, _maximumLength) : 
                    string.Format(_message, propertyName, _minimumLength, _maximumLength)
                    );
            }
            return null;
        }
    }
}