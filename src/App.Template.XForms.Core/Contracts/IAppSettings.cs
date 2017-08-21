using App.Template.XForms.Core.Options;

namespace App.Template.XForms.Core.Contracts
{
    public interface IAppSettings
    {
        BackendHost Identity { get; set; }
        string Key { get; }
        string ServiceId { get; set; }
        bool SetupFinished { get; set; }

        void ClearConfiguration();
        void Start();
    }
}