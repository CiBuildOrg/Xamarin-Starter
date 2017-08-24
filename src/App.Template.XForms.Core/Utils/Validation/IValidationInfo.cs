using System.Reflection;
using App.Template.XForms.Core.Contracts;
namespace App.Template.XForms.Core.Utils.Validation
{
    public interface IValidationInfo
    {
        MemberInfo Member { get; }
        IValidation Validation { get; }
        string[] Groups { get; }
    }
}