using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Utils.Auth;

namespace App.Template.XForms.Core.Contracts
{
    public interface IAuthenticationService
    {
        Task<bool> HasAlreadyRegistered(CancellationToken cancellationToken);
        Task<GetAccessTokenResponse> GetAccessToken(string username, string password, CancellationToken cancellationToken);
        Task<GetAccessTokenResponse> GetAccessToken(CancellationToken cancellationToken);
        Task<GetAccessTokenResponse> RefreshToken(CancellationToken cancellationToken);
    }
}