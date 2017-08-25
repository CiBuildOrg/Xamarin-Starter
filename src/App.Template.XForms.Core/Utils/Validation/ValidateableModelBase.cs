using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using App.Template.XForms.Core.Annotations;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Models;
using FluentValidation.Results;

namespace App.Template.XForms.Core.Utils.Validation
{
    public abstract class ValidateableModelBase<T, TK> : INotifyPropertyChanged, INotifyDataErrorInfo, IValidate
        where T: class where TK : AbstractValidator<T>
    {
        private readonly ValidationTemplate<ValidateableModelBase<T, TK>, TK> _validationTemplate;
        protected ValidateableModelBase()
        {
            _validationTemplate = new ValidationTemplate<ValidateableModelBase<T, TK>, TK>(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string this[string columnName] => _validationTemplate[columnName];

        public IEnumerable GetErrors(string propertyName)
        {
            return _validationTemplate.GetErrors(propertyName);
        }

        public bool HasErrors => _validationTemplate.HasErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add => _validationTemplate.ErrorsChanged += value;
            remove => _validationTemplate.ErrorsChanged -= value;
        }

        public string Error => _validationTemplate.Error;

        public ValidateResult ValidateModel()
        {
            var properties = GetType().GetTypeInfo().GetProperties();
            var result = new ValidateResult
            {
                Failures = new List<ValidationFailure>()
            };

            foreach (var propertyDefinition in properties)
            {
                if (propertyDefinition.GetCustomAttribute<ValidateableAttribute>() != null)
                {
                    var errors = _validationTemplate.GetErrors(propertyDefinition.Name).Cast<string>().ToList();
                    if (errors.Any())
                    {
                          errors.ForEach(x => result.Failures.Add(new ValidationFailure(propertyDefinition.Name, x)));
                    }
                }
            }

            return result;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual bool SetProperty<TP>(ref TP storage, TP value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TP>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}