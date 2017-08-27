using System.Diagnostics.CodeAnalysis;
using System.Threading;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Extensions;
using App.Template.XForms.Core.ViewModels;
using MvvmCross.Core.ViewModels;

namespace App.Template.XForms.Core.Startup
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class AppStart : MvxNavigatingObject,
        IMvxAppStart
    {
        private readonly IAuthenticationService _authenticationService;

        public AppStart(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        #region Interface: IMvxAppStart

        public void Start(object hint = null)
        {
            if (!_authenticationService.HasAlreadyRegistered(CancellationToken.None).WaitAsync().Result)
            {
                ShowViewModel<LoginViewModel>();
            }
            else
            {
                ShowViewModel<MenuViewModel>();
            }
        }

        #endregion
    }
}
