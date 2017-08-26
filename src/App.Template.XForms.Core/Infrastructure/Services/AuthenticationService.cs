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

        public async Task<bool> HasAlreadyRegistered(CancellationToken cancellationToken)
        {
            var hasToken = await _accessTokenStore.HasAccessToken(_appSettings.Identity.ClientId,
                _appSettings.ServiceId, cancellationToken).ConfigureAwait(false);

            return hasToken;
        }

        public async Task<GetAccessTokenResponse> GetAccessToken(string username, string password, CancellationToken cancellationToken)
        {
            var accessTokenClient = new AccessTokenClient(new OAuthServerConfiguration(_appSettings.Identity.Host, "/auth/token",
                _appSettings.Identity.ClientId, _appSettings.Identity.ClientSecret));

            var tokenResponse = await accessTokenClient.GetUserAccessToken(username, password, "", cancellationToken).ConfigureAwait(false);
            if (tokenResponse.IsError)
            {
                return new GetAccessTokenResponse { AccessToken = null, Error = tokenResponse.Error };
            }

            var token = tokenResponse.AccessToken;

            await _accessTokenStore.SaveClientAccessToken(_appSettings.Identity.ClientId, _appSettings.ServiceId,
                token, cancellationToken);

            return new GetAccessTokenResponse { AccessToken = token };
        }

        public async Task<GetAccessTokenResponse> GetAccessToken(CancellationToken cancellationToken)
        {
            var token = await _accessTokenStore.GetClientAccessToken(_appSettings.Identity.ClientId, _appSettings.ServiceId,
                cancellationToken);

            return token;
        }

        public async Task<GetAccessTokenResponse> RefreshToken(CancellationToken cancellationToken)
        {
            var tokenResponse = await _accessTokenStore.GetClientAccessToken(_appSettings.Identity.ClientId,
                _appSettings.ServiceId,
                cancellationToken).ConfigureAwait(false);

            if (!tokenResponse.HasAccessToken)
            {
                return new GetAccessTokenResponse {AccessToken = null, Error = tokenResponse.Error};
            }

            var token = tokenResponse.AccessToken;
            if (!token.ShouldBeRefreshed(TimeSpan.FromMinutes(2)))
                return new GetAccessTokenResponse {AccessToken = token, Error = null};

            var accessTokenClient = new AccessTokenClient(new OAuthServerConfiguration(
                _appSettings.Identity.Host,
                "/auth/token",
                _appSettings.Identity.ClientId, _appSettings.Identity.ClientSecret));

            try
            {
                var newTokenResponse = await accessTokenClient
                    .RefreshToken(token.RefreshToken, cancellationToken).ConfigureAwait(false);
                if (newTokenResponse.IsSuccess)
                {
                    var newToken = newTokenResponse.AccessToken;

                    await _accessTokenStore.SaveClientAccessToken(_appSettings.Identity.ClientId,
                        _appSettings.ServiceId,
                        newToken, cancellationToken).ConfigureAwait(false);

                    return new GetAccessTokenResponse {AccessToken = newToken, Error = null};
                }
                return new GetAccessTokenResponse
                {
                    AccessToken = null,
                    Error = newTokenResponse.Error
                };
            }
            catch (Exception exception)
            {
                return new GetAccessTokenResponse
                {
                    AccessToken = null,
                    Error = new ErrorResult
                    {
                        Error = "genericerror",
                        ErrorDescription = exception.Message
                    }
                };
            }
        }
    }
}
