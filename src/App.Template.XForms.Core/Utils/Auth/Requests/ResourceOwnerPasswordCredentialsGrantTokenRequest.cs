using System.Collections.Generic;
using Validation;

namespace App.Template.XForms.Core.Utils.Auth.Requests
{

    /// <summary>
    /// A request for a token based on a resource owner's password credentials. Implements: http://tools.ietf.org/html/rfc6749#section-4.3
    /// </summary>
    internal class ResourceOwnerPasswordCredentialsGrantTokenRequest : TokenRequest
    {
        private const string PasswordGrantType = "password";

        private readonly string _username;
        private readonly string _password;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _scope;
        private readonly Dictionary<string, string> _extra;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOwnerPasswordCredentialsGrantTokenRequest"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="extra"></param>
        /// <exception cref="System.ArgumentNullException">
        /// username
        /// or
        /// password
        /// or
        /// clientId
        /// </exception>
        public ResourceOwnerPasswordCredentialsGrantTokenRequest(string username, string password, string clientId, string clientSecret, string scope, Dictionary<string,string> extra = null)
        {
            Requires.NotNullOrEmpty(username, "username");
            Requires.NotNullOrEmpty(password, "password");
            Requires.NotNullOrEmpty(clientId, "clientId");
            Requires.NotNullOrEmpty(clientId, "clientSecret");

            _username = username;
            _password = password;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _scope = scope;
            _extra = extra;
        }

        /// <summary>
        /// Gets the parameters representing the request.
        /// </summary>
        /// <returns>
        /// The parameters.
        /// </returns>
        protected override NameValueCollection GetParameters()
        {
            var parameters = new NameValueCollection
                               {
                                   { "grant_type", PasswordGrantType },
                                   { "client_id", _clientId },
                                   { "client_secret", _clientSecret },
                                   { "username", _username },
                                   { "password", _password },
                                   { "scope", _scope }
                               };
            foreach(var s in _extra) parameters.Add(s.Key, s.Value);

            return parameters;
        }
    }
}