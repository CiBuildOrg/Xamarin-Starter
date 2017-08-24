using System;
using System.Linq.Expressions;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class RequiredAttribute : ValidationAttribute
    {
        public RequiredAttribute(string message = null)
            : base(message)
        {
        }

        public override IValidation CreateValidation(Type valueType)
        {
            if (valueType == typeof(string))
                return new RequiredValidation<string>(v => !string.IsNullOrEmpty(v), Message);
            if (!valueType.IsByRef)
            {
                var parameterExpresssion = Expression.Parameter(valueType, "o");
                var functionType = typeof(Func<,>).MakeGenericType(valueType, typeof(bool));
                var function = Expression.Lambda(
                    functionType,
                    Expression.NotEqual(
                        parameterExpresssion,
                        Expression.Constant(Activator.CreateInstance(valueType))),
                    parameterExpresssion).Compile();
                return (IValidation)Activator.CreateInstance(typeof(RequiredValidation<>).MakeGenericType(valueType), function, Message);
            }
            if (valueType.IsByRef)
                return new RequiredValidation<object>(o => o != null, Message);
            throw new NotSupportedException("Required Validator for type " + valueType.Name + " is not supported.");
        }
    }
}