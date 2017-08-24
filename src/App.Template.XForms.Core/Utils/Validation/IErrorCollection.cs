using System.Collections.Generic;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation
{
    public interface IErrorCollection : ICollection<IErrorInfo>
    {
        bool IsValid { get; }
        string Format();
        string Collect();
    }
}