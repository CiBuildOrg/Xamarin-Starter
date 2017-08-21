using System;

namespace App.Template.XForms.Core.Options
{
    public class BackendHost
    {
        public int TimeOut { get; set; }
        public bool Secure { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        private Uri _url;
        public Uri GetUrl()
        {
            if (_url != null)
                return _url;

            var scheme = Secure ? "https://" : "http://";
            return _url = new UriBuilder(scheme, Host, Port).Uri;
        }
    }
}
