using Android.Content;
using MvvmCross.Binding;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Bindings;
using MvvmCross.Forms.Droid.Presenters;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using System.Collections.Generic;
using System.Reflection;
using App.Template.XForms.Android.Bootstrap;
using App.Template.XForms.Core.Bootstrapper;
using App.Template.XForms.Core.Bootstrapper.AutofacBootstrap;
using App.Template.XForms.Core.Contracts;
using Autofac;
using MvvmCross.Forms.Core;
using MvvmCross.Platform.IoC;

namespace App.Template.XForms.Android
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
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

        protected override void InitializeDebugServices()
        {
            Mvx.RegisterSingleton<IMvxTrace>(new MvxDebugTrace());
            base.InitializeDebugServices();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            //var presenter = new MvxFormsDroidPagePresenter();
            var presenter = new MvxFormsDroidMasterDetailPagePresenter(new MvxFormsApplication());
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

            return presenter;
        }

        protected override IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn =
                    new List<Assembly>(base.ValueConverterAssemblies)
                    {
                        typeof(MvxLanguageConverter).Assembly
                    };

                return toReturn;
            }
        }

        protected override IMvxIoCProvider CreateIocProvider()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<FormsPlatformModule>();
            containerBuilder.RegisterModule<DroidModule>();

            return new AutofacMvxIocProvider(containerBuilder.Build());
        }

        protected override void InitializeBindingBuilder()
        {
            var bindingBuilder = CreateBindingBuilder();
            RegisterBindingBuilderCallbacks();
            bindingBuilder.DoRegistration();
        }

        private new MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxFormsBindingBuilder();
        }

        protected override IMvxAndroidViewsContainer CreateViewsContainer(Context applicationContext)
        {
            var viewContainerEmpty = (IMvxViewsContainer) base.CreateViewsContainer(applicationContext);
            var viewsContainerInitialized = Core.App.LoadViewsContainer(viewContainerEmpty, 
                Mvx.Resolve<IMvxViewsContainerHelper>());

            return (IMvxAndroidViewsContainer) viewsContainerInitialized;
        }
    }
}