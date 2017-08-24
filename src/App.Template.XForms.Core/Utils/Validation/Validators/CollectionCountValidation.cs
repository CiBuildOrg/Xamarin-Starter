using System;
using System.Collections;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class CollectionCountValidation : IValidation
    {
        private readonly Func<int, bool> _predicate;
        private readonly string _message;
        private readonly object _maximum;
        private readonly object _minimum;

        public CollectionCountValidation(Func<int, bool> predicate, object minimum, object maximum, string message)
        {
            _predicate = predicate;
            _minimum = minimum;
            _maximum = maximum;
            _message = message;
        }

        public IErrorInfo Validate(string propertyName, object value, object subject)
        {
            if (value == null)
                return null;

            return Validate(propertyName, value as ICollection, subject);
        }

        public IErrorInfo Validate(string propertyName, ICollection value, object subject)
        {
            if (!_predicate(value?.Count ?? 0))
            {
                return new ErrorInfo(propertyName, _message == null ?
                    string.Format("The Count of {0} must between {1} and {2}", propertyName, _minimum, _maximum) :
                    string.Format(_message, propertyName, _minimum, _maximum)
                    );
            }
            return null;
        }
    }
}
