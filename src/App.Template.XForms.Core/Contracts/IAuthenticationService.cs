using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Utils.Auth;

namespace App.Template.XForms.Core.Contracts
{
    public interface IAuthenticationService
    {
        Task<bool> NeedsToAuthenticate(CancellationToken cancellationToken);
        Task<AccessToken> GetAccessToken(string username, string password, CancellationToken cancellationToken);
        Task<AccessToken> GetAccessToken(CancellationToken cancellationToken);
    }
}