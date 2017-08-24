﻿using System;
using System.Linq;
using System.Reflection;
using App.Template.XForms.Core.Contracts;
using MvvmCross.FieldBinding;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class NcFieldRangeValidation : IValidation
    {
        private readonly Func<INC<decimal>, bool> _predicate;
        private readonly string _message;
        private readonly object _maximum;
        private readonly object _minimum;

        public NcFieldRangeValidation(Func<INC<decimal>, bool> predicate, object minimum, object maximum, string message)
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

            decimal decValue = 0;
            var incValue = value.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == "Value").GetValue(value);
            if (incValue == null)
                return null;

            if (!decimal.TryParse(incValue?.ToString(), out decValue))
                throw new NotSupportedException("NCFieldRange Validator for type " + value.GetType().FullName + " is not supported.");

            return Validate(fieldName, new NC<decimal>(decValue), subject);
        }

        public IErrorInfo Validate(string fieldName, INC<decimal> value, object subject)
        {
            if (!_predicate(value))
            {
                return new ErrorInfo(fieldName, _message == null ? 
                    string.Format("The Range of {0} must between {1} and {2}", fieldName, _minimum, _maximum) : 
                    string.Format(_message, fieldName, _minimum, _maximum)
                    );
            }
            return null;
        }
    }
}