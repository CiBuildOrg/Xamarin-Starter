using System.Linq;
using System.Reflection;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class NcFieldShouldBeLongValidation : IValidation
    {
        private readonly string _message;

        public NcFieldShouldBeLongValidation(string message)
        {
            _message = message;
        }

        public IErrorInfo Validate(string fieldName, object value, object subject)
        {
            if (value == null)
                return null;

            var incValue = value.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == "Value").GetValue(value);
            long result;
            if (incValue == null || long.TryParse(incValue.ToString(), out result))
                return null;

            return new ErrorInfo(fieldName, _message ?? string.Format("{0} should be a valid number.", fieldName));
        }
    }
}