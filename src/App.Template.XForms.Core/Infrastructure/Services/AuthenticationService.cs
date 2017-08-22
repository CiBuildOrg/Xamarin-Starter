using System;
using System.Threading.Tasks;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Utils.Auth;

namespace App.Template.XForms.Core.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccessTokenStore _accessTokenStore;
        private readonly IAccessTokenClient _accessTokenClient;
        private readonly IAppSettings _appSettings;

        public AuthenticationService(IAccessTokenStore accessTokenStore, IAccessTokenClient accessTokenClient, IAppSettings appSettings)
        {
            _accessTokenStore = accessTokenStore;
            _accessTokenClient = accessTokenClient;
            _appSettings = appSettings;
        }

        public Task<bool> NeedsToAuthenticate()
        {
            throw new NotImplementedException();
        }

        public Task<AccessToken> GetAccessToken(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<AccessToken> GetAccessToken()
        {
            throw new NotImplementedException();
        }
    }
}
