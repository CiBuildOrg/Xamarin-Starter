using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Utils.Auth;
using App.Template.XForms.Core.Utils.Auth.Requests;
using Newtonsoft.Json;
using Validation;

namespace App.Template.XForms.Core.Infrastructure
{
    /// <summary>
    /// This client allows retrieval of access tokens through the OAuth 2 protocol (http://tools.ietf.org/html/rfc6749).
    /// </summary>
    public class AccessTokenClient : IAccessTokenClient
    {
        private readonly OAuthServerConfiguration _serverConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenClient"/> class.
        /// </summary>
        /// <param name="serverConfiguration">The client configuration.</param>
        /// <exception cref="System.ArgumentNullException">clientConfiguration</exception>
        public AccessTokenClient(OAuthServerConfiguration serverConfiguration)
        {
            Requires.NotNull(serverConfiguration, "clientConfiguration");

            _serverConfiguration = serverConfiguration;
        }

        /// <summary>
        /// Gets an access token for a client.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <returns>The access token retrieval task.</returns>
        /// <remarks>
        /// This method implements the client credentials grant workflow (http://tools.ietf.org/html/rfc6749#section-4.4)
        /// </remarks>
        public async Task<TokenResponse> GetClientAccessToken(string scope)
        {
            return await GetClientAccessToken(scope, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets an access token for a client.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The access token retrieval task.</returns>
        /// <remarks>
        /// This method implements the client credentials grant workflow (http://tools.ietf.org/html/rfc6749#section-4.4)
        /// </remarks>
        public async Task<TokenResponse> GetClientAccessToken(string scope, CancellationToken cancellationToken)
        {
            return await ExecuteAccessTokenRequest(
                new ClientCredentialsGrantTokenRequest(_serverConfiguration.ClientId, _serverConfiguration.ClientSecret,
                    scope), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets an access token for a user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="extra"></param>
        /// <returns>The access token retrieval task.</returns>
        /// <remarks>
        /// This method implements the resource owner password credentials grant workflow (http://tools.ietf.org/html/rfc6749#section-4.3)
        /// </remarks>
        public async Task<TokenResponse> GetUserAccessToken(string username, string password, string scope,
            Dictionary<string, string> extra = null)
        {
            return await GetUserAccessToken(username, password, scope, CancellationToken.None, extra).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets an access token for a user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="extra"></param>
        /// <returns>The access token retrieval task.</returns>
        /// <remarks>
        /// This method implements the resource owner password credentials grant workflow (http://tools.ietf.org/html/rfc6749#section-4.3)
        /// </remarks>
        public async Task<TokenResponse> GetUserAccessToken(string username, string password, string scope,
            CancellationToken cancellationToken, Dictionary<string, string> extra = null)
        {
            Requires.NotNullOrEmpty(username, "username");
            Requires.NotNullOrEmpty(password, "password");

            return await ExecuteAccessTokenRequest(
                new ResourceOwnerPasswordCredentialsGrantTokenRequest(username, password, _serverConfiguration.ClientId,
                _serverConfiguration.ClientSecret,
                    scope, extra), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Exchanged a refresh token for a new access token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method implements the refresh access token workflow (http://tools.ietf.org/html/rfc6749#section-6)
        /// </remarks>
        public async Task<TokenResponse> RefreshToken(string refreshToken)
        {
            return await RefreshToken(refreshToken, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Exchanged a refresh token for a new access token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method implements the refresh access token workflow (http://tools.ietf.org/html/rfc6749#section-6)
        /// </remarks>
        public async Task<TokenResponse> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            Requires.NotNullOrEmpty(refreshToken, "refreshToken");

            return await ExecuteAccessTokenRequest(
                new RefreshAccessTokenRequest(refreshToken, _serverConfiguration.ClientId,
                    _serverConfiguration.ClientSecret), cancellationToken).ConfigureAwait(false);
        }

        private async Task<TokenResponse> ExecuteAccessTokenRequest(TokenRequest tokenRequest,
            CancellationToken cancellationToken)
        {
            var url = _serverConfiguration.BaseUrl  + _serverConfiguration.TokensUrl;
            var content = await tokenRequest.MakeCall(url, cancellationToken).ConfigureAwait(false);
            var isSuccessContent = content.IsSuccess;

            if (!isSuccessContent)
            {
                var errorContentArray = Encoding.UTF8.GetBytes(content.Content);
                using (var stream = new MemoryStream(errorContentArray))
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        var serializer = new JsonSerializer();
                        var error = serializer.Deserialize<ErrorResult>(new JsonTextReader(streamReader));

                        return new TokenResponse
                        {
                            AccessToken = null,
                            Error = error
                        };
                    }
                }
            
            }

            var successContentArray = Encoding.UTF8.GetBytes(content.Content);
            using (var stream = new MemoryStream(successContentArray))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    var serializer = new JsonSerializer();
                    var accessTokenResponse = serializer.Deserialize<AccessTokenResponse>(new JsonTextReader(streamReader));
                    var accessToken = accessTokenResponse.ToAccessToken();
                    return new TokenResponse
                    {
                        AccessToken = accessToken,
                        Error = null
                    };
                }
            }
        }
    }
}