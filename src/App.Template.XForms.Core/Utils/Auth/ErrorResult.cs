using Newtonsoft.Json;

namespace App.Template.XForms.Core.Utils.Auth
{
    public class  ErrorResult
    {
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription { get; set; }
    }
}