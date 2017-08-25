using System;
using Autofac;
using FluentValidation;

namespace App.Template.XForms.Core.Utils.Validation
{
    public class AutofacValidatorFactory : ValidatorFactoryBase
    {
        private readonly IComponentContext _context;

        public AutofacValidatorFactory(IComponentContext context)
        {
            _context = context;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            if (!_context.TryResolve(validatorType, out object instance)) return null;

            var validator = instance as IValidator;
            return validator;
        }
    }
}