using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Exceptions;
using App.Template.XForms.Core.Utils.Auth;
using App.Template.XForms.Core.Utils.Auth.Requests;
using RestSharp.Portable;
using RestSharp.Portable.Deserializers;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable.Serializers;
using Validation;

namespace App.Template.XForms.Core.Infrastructure
{
    /// <summary>
    /// This client allows retrieval of access tokens through the OAuth 2 protocol (http://tools.ietf.org/html/rfc6749).
    /// </summary>
    public class AccessTokenClient : IAccessTokenClient
    {
        private readonly OAuthServerConfiguration _serverConfiguration;
        private readonly JsonDeserializer _jsonDeserializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenClient"/> class.
        /// </summary>
        /// <param name="serverConfiguration">The client configuration.</param>
        /// <exception cref="System.ArgumentNullException">clientConfiguration</exception>
        public AccessTokenClient(OAuthServerConfiguration serverConfiguration)
        {
            Requires.NotNull(serverConfiguration, "clientConfiguration");

            _jsonDeserializer = new JsonDeserializer();
            _serverConfiguration = serverConfiguration;
            RestClient = new RestClient
            {
                BaseUrl = new Uri(serverConfiguration.BaseUrl)
            };
        }

        /// <summary>
        /// Gets the rest client used to make the requests.
        /// </summary>
        /// <value>
        /// The rest client.
        /// </value>
        public RestClient RestClient { get; }

        /// <summary>
        /// Gets an access token for a client.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <returns>The access token retrieval task.</returns>
        /// <remarks>
        /// This method implements the client credentials grant workflow (http://tools.ietf.org/html/rfc6749#section-4.4)
        /// </remarks>
        public Task<AccessToken> GetClientAccessToken(string scope)
        {
            return GetClientAccessToken(scope, CancellationToken.None);
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
        public Task<AccessToken> GetClientAccessToken(string scope, CancellationToken cancellationToken)
        {
            return ExecuteAccessTokenRequest(
                new ClientCredentialsGrantTokenRequest(_serverConfiguration.ClientId, _serverConfiguration.ClientSecret,
                    scope), cancellationToken);
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
        public Task<AccessToken> GetUserAccessToken(string username, string password, string scope,
            Dictionary<string, string> extra = null)
        {
            return GetUserAccessToken(username, password, scope, CancellationToken.None, extra);
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
        public Task<AccessToken> GetUserAccessToken(string username, string password, string scope,
            CancellationToken cancellationToken, Dictionary<string, string> extra = null)
        {
            Requires.NotNullOrEmpty(username, "username");
            Requires.NotNullOrEmpty(password, "password");

            return ExecuteAccessTokenRequest(
                new ResourceOwnerPasswordCredentialsGrantTokenRequest(username, password, _serverConfiguration.ClientId,
                    scope, extra), cancellationToken);
        }

        /// <summary>
        /// Exchanged a refresh token for a new access token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method implements the refresh access token workflow (http://tools.ietf.org/html/rfc6749#section-6)
        /// </remarks>
        public Task<AccessToken> RefreshToken(string refreshToken)
        {
            return RefreshToken(refreshToken, CancellationToken.None);
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
        public Task<AccessToken> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            Requires.NotNullOrEmpty(refreshToken, "refreshToken");

            return ExecuteAccessTokenRequest(
                new RefreshAccessTokenRequest(refreshToken, _serverConfiguration.ClientId,
                    _serverConfiguration.ClientSecret), cancellationToken);
        }

        // might not work?
        //public Task<T> ExecutePostAsync<T, TK>(TK request, CancellationToken cancellationToken)
        //{
            
        //    var restClient = new RestClient
        //    {
        //        BaseUrl =new Uri("http://///....")
        //    };

        //    var clientRequest = new RestRequest
        //    {
        //        Method = Method.POST,
        //    };

        //    clientRequest.AddHeader("Content-Type", "application/json");
        //    clientRequest.Serializer = new JsonSerializer
        //    {
        //        ContentType = "application/json"
        //    };

        //    clientRequest.AddBody(request);
        //    return restClient.ExecuteAsync(clientRequest, cancellationToken)
        //        .ContinueWith(t =>
        //        {
        //            // We will only process HTTP 200 status codes, as only then we can be sure 
        //            // that we have received a correct response and can be try to deserialize
        //            // the access token response
        //            if (t.Result.StatusCode == HttpStatusCode.OK)
        //            {
        //                return _jsonDeserializer.Deserialize<T>(t.Result);
        //            }

        //            throw new HttpException((int)t.Result.StatusCode, t.Result.StatusDescription);
        //        }, cancellationToken);
        //}

        private Task<AccessToken> ExecuteAccessTokenRequest(TokenRequest tokenRequest,
            CancellationToken cancellationToken)
        {
            var restRequest = tokenRequest.ToRestRequest(_serverConfiguration.TokensUrl);

            return RestClient.ExecuteAsync(restRequest, cancellationToken)
                .ContinueWith(t =>
                {
                    // We will only process HTTP 200 status codes, as only then we can be sure 
                    // that we have received a correct response and can be try to deserialize
                    // the access token response
                    if (t.Result.StatusCode == HttpStatusCode.OK)
                    {
                        return _jsonDeserializer.Deserialize<AccessTokenResponse>(t.Result).ToAccessToken();
                    }

                    throw new HttpException((int) t.Result.StatusCode, t.Result.StatusDescription);
                }, cancellationToken);
        }
    }
}