using FluentValidation;

namespace App.Template.XForms.Core.Utils.Validation
{
    public static class ValidationFactory
    {
        public static IValidator<TK> GetValidator<T, TK>() where T : AbstractValidator<TK> where TK : class 
        {
            return >();
        }
    }
}