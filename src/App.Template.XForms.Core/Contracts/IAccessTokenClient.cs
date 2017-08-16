using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Utils.Auth;

namespace App.Template.XForms.Core.Contracts
{
    public interface IAccessTokenClient
    {
        Task<AccessToken> GetClientAccessToken(string scope);
        Task<AccessToken> GetClientAccessToken(string scope, CancellationToken cancellationToken);

        Task<AccessToken> GetUserAccessToken(string username, string password, string scope,
            Dictionary<string, string> extra = null);

        Task<AccessToken> GetUserAccessToken(string username, string password, string scope,
            CancellationToken cancellationToken, Dictionary<string, string> extra = null);

        Task<AccessToken> RefreshToken(string refreshToken);
        Task<AccessToken> RefreshToken(string refreshToken, CancellationToken cancellationToken);
    }
}