using ImageCircle.Forms.Plugin.iOS;
using MvvmCross.Binding;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.iOS.Presenters;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using System.Collections.Generic;
using System.Reflection;
using App.Template.XForms.Core.Bootstrapper;
using App.Template.XForms.Core.Bootstrapper.AutofacBootstrap;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.iOS.Bootstrap;
using Autofac;
using MvvmCross.Platform.IoC;
using UIKit;
using Xamarin.Forms;

namespace App.Template.XForms.iOS
{
    public class Setup : MvxIosSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxIoCProvider CreateIocProvider()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<FormsPlatformModule>();
            containerBuilder.RegisterModule<IosModule>();

            return new AutofacMvxIocProvider(containerBuilder.Build());
        }

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            Forms.Init();
            ImageCircleRenderer.Init();

            var xamarinFormsApp = new MvxFormsApplication();
            //var presenter = new MvxFormsIosPagePresenter(Window, xamarinFormsApp);
            var presenter = new MvxFormsIosMasterDetailPagePresenter(Window, xamarinFormsApp);
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);
            return presenter;
        }

        protected override IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn =
                    new List<Assembly>(base.ValueConverterAssemblies) {typeof(MvxLanguageConverter).Assembly};
                return toReturn;
            }
        }

        protected override void InitializeBindingBuilder()
        {
            var bindingBuilder = CreateBindingBuilder();

            RegisterBindingBuilderCallbacks();
            bindingBuilder.DoRegistration();
        }

        private new static MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxFormsBindingBuilder();
        }

        protected sealed override IMvxIosViewsContainer CreateIosViewsContainer()
        {
            var viewsContainer = Core.App.LoadViewsContainer(base.CreateIosViewsContainer(), 
                Mvx.Resolve<IMvxViewsContainerHelper>());

            return (IMvxIosViewsContainer) viewsContainer;
        }
    }
}