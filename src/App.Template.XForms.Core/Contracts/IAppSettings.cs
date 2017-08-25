using App.Template.XForms.Core.Options;

namespace App.Template.XForms.Core.Contracts
{
    public interface IAppSettings
    {
        BackendHost Identity { get;}
        string Key { get; }
        string ServiceId { get; }
        string Version { get; }
        bool SetupFinished { get; }
        Security Security { get; }
        void ClearConfiguration();
        void Start();
        Options.Validation Validation { get;}
    }
}