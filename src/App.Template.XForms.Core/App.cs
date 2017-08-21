using System.Diagnostics.CodeAnalysis;
using App.Template.XForms.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.MvvmCross;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platform;

namespace App.Template.XForms.Core
{
    public class App : MvxApplication
    {
        [SuppressMessage("ReSharper", "NotAccessedField.Local")] private readonly IAppSettings _settings;

        public App(IAppSettings settings)
        {
            _settings = settings;
        }

        public override void Initialize()
        {
            Mvx.RegisterSingleton<IMvxFormsPageLoader>(new MvxFormsViewLoader());

            RegisterAppStart<MenuViewModel>();
        }

        public static IMvxViewsContainer LoadViewsContainer(IMvxViewsContainer viewsContainer, IMvxViewsContainerHelper viewViewModelBagService)
        {
            foreach (var bag in viewViewModelBagService.GetViewViewModelCorrespondenceMap())
            {
                viewsContainer.Add(bag.ViewModel, bag.View);
            }

            return viewsContainer;
        }
    }
}