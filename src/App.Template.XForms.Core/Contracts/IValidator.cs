namespace App.Template.XForms.Core.Contracts
{
    public interface IValidator
    {
        IErrorCollection Validate(object subject, string group = null);
        bool IsRequired(object subject, string memberName);
    }
}