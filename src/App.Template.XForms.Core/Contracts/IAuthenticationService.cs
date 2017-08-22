using System.Threading.Tasks;
using App.Template.XForms.Core.Utils.Auth;

namespace App.Template.XForms.Core.Contracts
{
    public interface IAuthenticationService
    {
        Task<bool> NeedsToAuthenticate();
        Task<AccessToken> GetAccessToken(string username, string password);
        Task<AccessToken> GetAccessToken();
    }
}