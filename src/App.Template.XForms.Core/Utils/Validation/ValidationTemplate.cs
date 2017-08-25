using System;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using FluentValidation;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace App.Template.XForms.Core.Utils.Validation
{
    public class ValidationTemplate :  INotifyDataErrorInfo
    {
        private readonly INotifyPropertyChanged _target;
        private readonly IValidator _validator;
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IValidator> Validators = new ConcurrentDictionary<RuntimeTypeHandle, IValidator>();

        private ValidationResult _validationResult;


        public ValidationTemplate(INotifyPropertyChanged target)
        {
            this._target = target;
            _validator = GetValidator(target.GetType());
            _validationResult = _validator.Validate(target);
            target.PropertyChanged += Validate;
        }

        private static IValidator GetValidator(Type modelType)
        {
            IValidator validator;
            if (Validators.TryGetValue(modelType.TypeHandle, out validator)) return validator;
            var typeName = string.Format("{0}.{1}Validator", modelType.Namespace, modelType.Name);
            var type = modelType.GetTypeInfo().Assembly.GetType(typeName, true);
            Validators[modelType.TypeHandle] = validator = (IValidator)Activator.CreateInstance(type);
            return validator;
        }

        void Validate(object sender, PropertyChangedEventArgs e)
        {
            _validationResult = _validator.Validate(_target);
            foreach (var error in _validationResult.Errors)
            {
                RaiseErrorsChanged(error.PropertyName);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _validationResult.Errors
                .Where(x => x.PropertyName == propertyName)
                .Select(x => x.ErrorMessage);
        }

        public bool HasErrors => _validationResult.Errors.Count > 0;

        public string Error
        {
            get
            {
                var strings = _validationResult.Errors.Select(x => x.ErrorMessage)
                    .ToArray();
                return string.Join(Environment.NewLine, strings);
            }
        }

        public string this[string propertyName]
        {
            get
            {
                var strings = _validationResult.Errors.Where(x => x.PropertyName == propertyName)
                    .Select(x => x.ErrorMessage)
                    .ToArray();
                return string.Join(Environment.NewLine, strings);
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void RaiseErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            handler?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
