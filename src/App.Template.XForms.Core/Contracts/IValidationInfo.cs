using System.Reflection;

namespace App.Template.XForms.Core.Contracts
{
    public interface IValidationInfo
    {
        MemberInfo Member { get; }
        IValidation Validation { get; }
        string[] Groups { get; }
    }
}