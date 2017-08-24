using System.Collections.Generic;

namespace App.Template.XForms.Core.Contracts
{
    public interface IErrorCollection : ICollection<IErrorInfo>
    {
        bool IsValid { get; }
        string Format();
        string Collect();
    }
}