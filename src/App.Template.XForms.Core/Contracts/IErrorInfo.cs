namespace App.Template.XForms.Core.Contracts
{
    public interface IErrorInfo
    {
        string MemberName { get; }
        string Message { get; }
    }
}