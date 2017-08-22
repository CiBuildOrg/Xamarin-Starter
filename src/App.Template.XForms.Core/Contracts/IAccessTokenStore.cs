using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Utils.Auth;

namespace App.Template.XForms.Core.Contracts
{
    public interface IAccessTokenStore
    {
        Task<bool> HasAccessToken(string clientId, string serviceId, CancellationToken cancellationToken);
        Task<AccessToken> GetClientAccessToken(string clientId, string serviceId, CancellationToken cancellationToken);
        Task<AccessToken> GetUserAccessToken(string username, string serviceId, CancellationToken cancellationToken);

        Task SaveClientAccessToken(string clientId, string serviceId, AccessToken accessToken,
            CancellationToken cancellationToken);

        Task SaveUserAccessToken(string username, string serviceId, AccessToken accessToken,
            CancellationToken cancellationToken);
    }
}