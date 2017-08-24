using System.Collections.ObjectModel;
using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Utils.Validation
{
    public class ValidationCollection : Collection<IValidationInfo>, IValidationCollection
    {
    }
}