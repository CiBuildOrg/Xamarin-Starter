using System;
using System.Linq.Expressions;
using App.Template.XForms.Core.Contracts;
using MvvmCross.FieldBinding;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class NcFieldRequiredAttribute : NcFieldValidationAttribute
    {
        public NcFieldRequiredAttribute(string message = null)
            : base(message)
        {
        }

        public override IValidation CreateValidation(Type valueType)
        {
            if (!valueType.FullName.Contains("MvvmCross.FieldBinding"))
                throw new NotSupportedException("NCFieldRequired Validator for type " + valueType.Name + " is not supported.");

            var genericityType = valueType.GenericTypeArguments[0];
            if (genericityType == null || genericityType == typeof(string))
                return new NcFieldRequiredValidation<INC<string>>(v => v == null || !string.IsNullOrEmpty(v.Value), Message);

            if (!genericityType.IsByRef)
            {
                var parameterExpresssion = Expression.Parameter(genericityType, "o");
                var functionType = typeof(Func<,>).MakeGenericType(genericityType, typeof(bool));
                var function = Expression.Lambda(
                    functionType,
                    Expression.NotEqual(
                        parameterExpresssion,
                        Expression.Constant(Activator.CreateInstance(genericityType))),
                    parameterExpresssion).Compile();
                return (IValidation)Activator.CreateInstance(typeof(NcFieldRequiredValidation<>).MakeGenericType(genericityType), function, Message);
            }
            if (genericityType.IsByRef)
                return new NcFieldRequiredValidation<object>(o => o != null, Message);
            throw new NotSupportedException("NCFieldRequired Validator for type " + genericityType.Name + " is not supported.");
        }
    }
}