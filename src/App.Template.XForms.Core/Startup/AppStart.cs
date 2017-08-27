using System.Diagnostics.CodeAnalysis;
using App.Template.XForms.Core.ViewModels;
using MvvmCross.Core.ViewModels;

namespace App.Template.XForms.Core.Startup
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class AppStart : MvxNavigatingObject,
        IMvxAppStart
    {
        #region Interface: IMvxAppStart

        public void Start(object hint = null)
        {
            ShowViewModel<LoginViewModel>();
        }

        #endregion
    }
}
