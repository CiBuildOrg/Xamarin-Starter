using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation.Validators
{
    public class ErrorInfo : IErrorInfo
    {
        public ErrorInfo(string memberName, string message)
        {
            Message = message;
            MemberName = memberName;
        }

        public string MemberName { get; }
        public string Message { get; }
    }
}