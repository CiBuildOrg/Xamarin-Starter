using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Utils.Auth;

namespace App.Template.XForms.Core.Infrastructure.Services
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccessTokenStore _accessTokenStore;
        private readonly IAppSettings _appSettings;

        public AuthenticationService(IAccessTokenStore accessTokenStore, IAppSettings appSettings)
        {
            _accessTokenStore = accessTokenStore;
            _appSettings = appSettings;
        }

        public async Task<bool> NeedsToAuthenticate(CancellationToken cancellationToken)
        {
            var hasToken = await _accessTokenStore.HasAccessToken(_appSettings.Identity.ClientId,
                _appSettings.ServiceId, cancellationToken).ConfigureAwait(false); 

            if (!hasToken)
                return true;

            var token =  await _accessTokenStore.GetClientAccessToken(_appSettings.Identity.ClientId, _appSettings.ServiceId,
                cancellationToken).ConfigureAwait(false);

            if (token.ShouldBeRefreshed(TimeSpan.FromMinutes(2)))
            {
                var accessTokenClient = new AccessTokenClient(new OAuthServerConfiguration(_appSettings.Identity.Host,
                    "/auth/token",
                    _appSettings.Identity.ClientId, _appSettings.Identity.ClientSecret));

                try
                {
                    var newToken = await accessTokenClient.RefreshToken(token.RefreshToken, cancellationToken).ConfigureAwait(false);
                    await  _accessTokenStore.SaveClientAccessToken(_appSettings.Identity.ClientId, _appSettings.ServiceId,
                        newToken, cancellationToken).ConfigureAwait(false);

                    return false;
                }
                catch
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<AccessToken> GetAccessToken(string username, string password, CancellationToken cancellationToken)
        {
            var accessTokenClient = new AccessTokenClient(new OAuthServerConfiguration(_appSettings.Identity.Host, "/auth/token", 
                _appSettings.Identity.ClientId, _appSettings.Identity.ClientSecret));

            var token = await accessTokenClient.GetUserAccessToken(username, password, "", cancellationToken).ConfigureAwait(false);
            await _accessTokenStore.SaveClientAccessToken(_appSettings.Identity.ClientId, _appSettings.ServiceId,
                token, cancellationToken);

            return token;
        }

        public async Task<AccessToken> GetAccessToken(CancellationToken cancellationToken)
        {
            var token =  await _accessTokenStore.GetClientAccessToken(_appSettings.Identity.ClientId, _appSettings.ServiceId,
                cancellationToken);

            return token;
        }
    }
}
