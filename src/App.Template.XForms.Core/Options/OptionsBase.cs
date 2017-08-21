using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App.Template.XForms.Core.Options
{
    public abstract class OptionsBase
    {
        /// <summary>
        /// Load from an embedded json ressource.
        /// </summary>
        protected static JObject LoadJObject(string relativePath = "App.Template.XForms.Core.Resources")
        {
            var name = !ResourceKeys.IsDebug
                ? $"{relativePath}.appsettings.json"
                : $"{relativePath}.appsettings.Development.json";

            var assembly = typeof(AppSettings).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream(name);

            if (stream == null && ResourceKeys.IsDebug)
                stream = assembly.GetManifestResourceStream($"{relativePath}.appsettings.json");
            else if (stream == null)
                return null;

            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<JObject>(json);
            }
        }
    }
}
