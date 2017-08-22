using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Utils.Auth;

namespace App.Template.XForms.Core.Infrastructure.Services
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccessTokenStore _accessTokenStore;
        private readonly IAppSettings _appSettings;

        public AuthenticationService(IAccessTokenStore accessTokenStore, IAppSettings appSettings)
        {
            _accessTokenStore = accessTokenStore;
            _appSettings = appSettings;
        }

        public bool NeedsToAuthenticate(CancellationToken cancellationToken)
        {
            var hasToken = _accessTokenStore.HasAccessToken(_appSettings.Identity.ClientId,
                _appSettings.ServiceId, cancellationToken).GetAwaiter().GetResult(); 
            if (!hasToken)
                return true;

            var token =  _accessTokenStore.GetClientAccessToken(_appSettings.Identity.ClientId, _appSettings.ServiceId,
                cancellationToken).GetAwaiter().GetResult();

            if (token.ShouldBeRefreshed(TimeSpan.FromMinutes(2)))
            {
                var accessTokenClient = new AccessTokenClient(new OAuthServerConfiguration(_appSettings.Identity.Host,
                    "/auth/token",
                    _appSettings.Identity.ClientId, _appSettings.Identity.ClientSecret));

                try
                {
                    var newToken = accessTokenClient.RefreshToken(token.RefreshToken, cancellationToken).GetAwaiter().GetResult();
                     _accessTokenStore.SaveClientAccessToken(_appSettings.Identity.ClientId, _appSettings.ServiceId,
                        newToken, cancellationToken).GetAwaiter().GetResult();

                    return false;
                }
                catch
                {
                    return true;
                }
            }
            else return false;
        }

        public AccessToken GetAccessToken(string username, string password, CancellationToken cancellationToken)
        {
            var accessTokenClient = new AccessTokenClient(new OAuthServerConfiguration(_appSettings.Identity.Host, "/auth/token", 
                _appSettings.Identity.ClientId, _appSettings.Identity.ClientSecret));

            var token =  accessTokenClient.GetUserAccessToken(username, password, "", cancellationToken).GetAwaiter().GetResult();
             _accessTokenStore.SaveClientAccessToken(_appSettings.Identity.ClientId, _appSettings.ServiceId,
                token, cancellationToken).GetAwaiter().GetResult();

            return token;
        }

        public AccessToken GetAccessToken(CancellationToken cancellationToken)
        {
            var token =  _accessTokenStore.GetClientAccessToken(_appSettings.Identity.ClientId, _appSettings.ServiceId,
                cancellationToken).GetAwaiter().GetResult();

            return token;
        }
    }
}
