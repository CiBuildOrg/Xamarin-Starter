using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Options
{
    public class ServiceSettings : OptionsBase, IServiceSettings
    {
        public string ServiceId { get; }
        public string Version { get; }
        public bool SetupFinished { get; }

        public ServiceSettings()
        {
            var obj = LoadJObject();
            //App
            var appToken = obj["App"];
            ServiceId = (string)appToken["ServiceId"];
            Version = (string)appToken["Version"];
            SetupFinished = (bool) appToken["SetupFinished"];
        }
    }
}