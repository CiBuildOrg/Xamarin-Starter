using App.Template.XForms.Core.Contracts;

namespace App.Template.XForms.Core.Options
{
    public class ServiceSettings : OptionsBase, IServiceSettings
    {
        public string ServiceId { get; }

        public ServiceSettings()
        {
            var obj = LoadJObject();
            //App
            var appToken = obj["App"];
            ServiceId = (string)appToken["ServiceId"];
        }
    }
}