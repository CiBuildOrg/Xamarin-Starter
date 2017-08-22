#define ALWAYS_LOAD

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using App.Template.XForms.Core.Contracts;
using Autofac;

namespace App.Template.XForms.Core.Options
{
    /// <summary>
    /// Holds all settings and preferences for this app.
    /// </summary>
    public class AppSettings : OptionsBase, IKeyProvider, IStartable, IAppSettings
    {
        public string Key => nameof(AppSettings);

        public bool SetupFinished { get; set; }
        public string ServiceId { get; set; }
        public string Version { get; set; }
        public BackendHost Identity { get; set; }
        public Security Security { get; set; }

        private readonly IKeyValueStore _store;
        private readonly IServiceSettings _keyProvider;

        public AppSettings(IKeyValueStore store, IServiceSettings keyProvider)
        {
            _store = store;
            _keyProvider = keyProvider;
            Identity = new BackendHost();
        }

        /// <summary>
        /// Restore the Configuration from the embedded appsettings.json
        /// </summary>
        public void ClearConfiguration()
        {
            LoadFromRessource();

            SetupFinished = false;
        }

        public void Start()
        {
            //var settings = _store.GetObservable<AppSettings>(nameof(AppSettings)).Wait();
            //if (settings != null)
            //{
            //    if(_keyProvider.Version != settings.Version)
            //    {
            //        LoadFromRessource();
            //        await Persist();
            //    }
            //    else
            //    {
            //        SetupFinished = settings.SetupFinished;
            //        ServiceId = settings.ServiceId;
            //        Identity = settings.Identity;
            //    }
            //}
            //else
            //{
                LoadFromRessource();
            //    await Persist();
            //}
        }

        /// <summary>
        /// Load from an embedded json ressource.
        /// </summary>
        private void LoadFromRessource(string relativePath = "App.Template.XForms.Core.Resources")
        {
            var intermediate = LoadJObject(relativePath);
            if (intermediate == null)
                throw new Exception("Could not read settings json file");
            //App
            var appToken = intermediate["App"];
            ServiceId = (string)appToken["ServiceId"];
            SetupFinished = (bool)appToken["SetupFinished"];

            //Backend
            var backendTokenIdentity = intermediate["Backend"]["Identity"];
            Identity = new BackendHost
            {
                Host = (string)backendTokenIdentity["Host"],
                Port = (int)backendTokenIdentity["Port"],
                Secure = (bool)backendTokenIdentity["Secure"],
                TimeOut = (int)backendTokenIdentity["TimeOut"],
                ClientId = (string)backendTokenIdentity["ClientId"],
                ClientSecret = (string)backendTokenIdentity["ClientSecret"]
            };

            var security = intermediate["Security"];
            Security = new Security
            {
                StorePassword = (string)security["StorePassword"]
            };
        }

        private async Task Persist()
        {
            await _store.Insert(this);
            await _store.Persist();
        }
    }
}