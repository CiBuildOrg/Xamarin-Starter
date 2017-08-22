using System.Threading;
using System.Threading.Tasks;
using App.Template.XForms.Core.Utils.Auth;

namespace App.Template.XForms.Core.Contracts
{
    public interface IAuthenticationService
    {
        bool NeedsToAuthenticate(CancellationToken cancellationToken);
        AccessToken GetAccessToken(string username, string password, CancellationToken cancellationToken);
        AccessToken GetAccessToken(CancellationToken cancellationToken);
    }
}