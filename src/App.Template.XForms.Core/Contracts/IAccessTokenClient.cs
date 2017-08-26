using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Utils.Auth;

namespace App.Template.XForms.Core.Contracts
{
    public interface IAccessTokenClient
    {
        Task<TokenResponse> GetClientAccessToken(string scope);
        Task<TokenResponse> GetClientAccessToken(string scope, CancellationToken cancellationToken);

        Task<TokenResponse> GetUserAccessToken(string username, string password, string scope,
            Dictionary<string, string> extra = null);

        Task<TokenResponse> GetUserAccessToken(string username, string password, string scope,
            CancellationToken cancellationToken, Dictionary<string, string> extra = null);

        Task<TokenResponse> RefreshToken(string refreshToken);
        Task<TokenResponse> RefreshToken(string refreshToken, CancellationToken cancellationToken);
    }
}   