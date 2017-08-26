using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Utils.Auth;
using Validation;
using Xamarin.Auth;

namespace App.Template.XForms.Core.Infrastructure
{
    /// <summary>
    /// A store to securely store <see cref="AccessToken"/> instances in. This class acts as a wrapper around
    /// the <see cref="AccountStore"/> class, which is where the actual storage takes place.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class AccessTokenStore : IAccessTokenStore
    {
        private const string NormalizedUsernamePrefix = "user:";
        private const string NormalizedClientIdPrefix = "client:";

        private readonly AccountStore _accountStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenStore"/> class.
        /// </summary>
        /// <param name="accountStore">The account store.</param>
        /// <exception cref="System.ArgumentNullException">accountStore</exception>
        public AccessTokenStore(AccountStore accountStore)
        {
            Requires.NotNull(accountStore, "accountStore");

            _accountStore = accountStore;
        }

        /// <summary>
        /// Gets the user access token.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="serviceId">The service id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task that represents the token retrieval action.</returns>
        /// <exception cref="System.ArgumentNullException">username
        /// or
        /// serviceId</exception>
        public async Task<GetAccessTokenResponse> GetUserAccessToken(string username, string serviceId,
            CancellationToken cancellationToken)
        {
            Requires.NotNullOrEmpty(username, "username");
            Requires.NotNullOrEmpty(serviceId, "serviceId");

            return await GetAccessToken(NormalizeUsername(username), serviceId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> HasAccessToken(string clientId, string serviceId, CancellationToken cancellationToken)
        {
            return await Task.Factory.StartNew(() =>
            {
                var normalizedUsername = NormalizeClientId(clientId);
                var account = _accountStore.FindAccountsForService(serviceId).FirstOrDefault(a => string.Equals(a.Username, normalizedUsername,
                    StringComparison.CurrentCultureIgnoreCase));
                return account != null;
            }, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the client access token.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="serviceId">The service id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task that represents the token retrieval action.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">clientId
        /// or
        /// serviceId</exception>
        public async Task<GetAccessTokenResponse> GetClientAccessToken(string clientId, string serviceId,
            CancellationToken cancellationToken)
        {
            Requires.NotNullOrEmpty(clientId, "clientId");
            Requires.NotNullOrEmpty(serviceId, "serviceId");
            return await GetAccessToken(NormalizeClientId(clientId), serviceId, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the user access token.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="serviceId">The service id.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task that represents the save action.</returns>
        public async Task SaveUserAccessToken(string username, string serviceId, AccessToken accessToken,
            CancellationToken cancellationToken)
        {
            Requires.NotNullOrEmpty(username, "username");
            Requires.NotNullOrEmpty(serviceId, "serviceId");
            Requires.NotNull(accessToken, "accessToken");
            await SaveAccessToken(NormalizeUsername(username), serviceId, accessToken, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the client access token.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="serviceId">The service id.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task that represents the save action.</returns>
        public async Task SaveClientAccessToken(string clientId, string serviceId, AccessToken accessToken,
            CancellationToken cancellationToken)
        {
            Requires.NotNullOrEmpty(clientId, "clientId");
            Requires.NotNullOrEmpty(serviceId, "serviceId");
            Requires.NotNull(accessToken, "accessToken");

            await SaveAccessToken(NormalizeClientId(clientId), serviceId, accessToken, cancellationToken).ConfigureAwait(false);
        }

        private async Task<GetAccessTokenResponse> GetAccessToken(string normalizedUsername, string serviceId,
            CancellationToken cancellationToken)
        {
            return await Task.Factory.StartNew(() =>
            {
                var account = _accountStore.FindAccountsForService(serviceId).FirstOrDefault(a => string.Equals(a.Username, normalizedUsername,
                        StringComparison.CurrentCultureIgnoreCase));

                if (account == null)
                {
                    return new GetAccessTokenResponse
                    {
                        AccessToken = null,
                        Error = new ErrorResult
                        {
                            Error = "tokennotfound",
                            ErrorDescription = "Login information not found"
                        }
                    };
                }

                return new GetAccessTokenResponse
                {
                    AccessToken = new AccessToken(account.Properties),
                    Error = null
                };
            }, cancellationToken).ConfigureAwait(false);
        }

        private async Task SaveAccessToken(string normalizedUsername, string serviceId, AccessToken accessToken,
            CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    var account = _accountStore.FindAccountsForService(serviceId).FirstOrDefault(a => string.Equals(a.Username, normalizedUsername,
                        StringComparison.CurrentCultureIgnoreCase));

                    if (account != null)
                    {
                        _accountStore.Delete(account, serviceId);
                    }
                    _accountStore.Save(new Account(normalizedUsername, accessToken.ToDictionary()), serviceId);
                },
                cancellationToken).ConfigureAwait(false);
        }

        private static string NormalizeUsername(string username)
        {
            return NormalizedUsernamePrefix + username;
        }

        private static string NormalizeClientId(string clientId)
        {
            return NormalizedClientIdPrefix + clientId;
        }
    }
}