using Validation;

namespace App.Template.XForms.Core.Utils.Auth
{
    /// <summary>
    /// The configuration that describes the OAuth server configuration.
    /// </summary>
    public class OAuthServerConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuthServerConfiguration" /> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="tokensUrl">The URL to which token requests will be made.</param>
        /// <param name="clientId">The client id.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <exception cref="System.ArgumentNullException">clientId
        /// or
        /// clientSecret</exception>
        public OAuthServerConfiguration(string baseUrl, string tokensUrl, string clientId, string clientSecret)
        {
            Requires.NotNull(baseUrl, "baseUrl");
            Requires.NotNull(tokensUrl, "tokensUrl");
            Requires.NotNullOrEmpty(clientId, "clientId");
            Requires.NotNullOrEmpty(clientSecret, "clientSecret");

            BaseUrl = baseUrl;
            TokensUrl = tokensUrl;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        /// <summary>
        /// Gets the base URL of the OAuth server.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        public string BaseUrl { get; }

        /// <summary>
        /// Gets the URL to which token requests will be made.
        /// </summary>
        /// <value>
        /// The tokens URL.
        /// </value>
        public string TokensUrl { get; }

        /// <summary>
        /// Gets the client id.
        /// </summary>
        /// <value>
        /// The client id.
        /// </value>
        public string ClientId { get; }

        /// <summary>
        /// Gets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret { get; }
    }
}