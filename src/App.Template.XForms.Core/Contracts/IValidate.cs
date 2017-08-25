using App.Template.XForms.Core.Models;

namespace App.Template.XForms.Core.Contracts
{
    public interface IValidate
    {
        ValidateResult ValidateModel();
    }
}