using System.Linq;
using System.Reflection;
using App.Template.XForms.Core.Contracts;
using App.Template.XForms.Core.Infrastructure;
using App.Template.XForms.Core.Infrastructure.Services;
using App.Template.XForms.Core.MvvmCross;
using App.Template.XForms.Core.Options;
using Autofac;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platform;

namespace App.Template.XForms.Core.Bootstrapper
{
    public class FormsPlatformModule : Autofac.Module
    {
        private const string ServicesEnding = "Service";

        protected override void Load(ContainerBuilder builder)
        {
            // register any types that live in the platform PCL
            builder
                .RegisterType<MvxFormsViewLoader>()
                .As<IMvxFormsPageLoader>();

            builder.RegisterAssemblyTypes(CoreAssemblyHelper.CoreAssembly)
                .Where(t => t.GetTypeInfo().IsClass && t.Name.EndsWith(ServicesEnding))
                .As(t => t.GetInterfaces().Single(i => i.Name.EndsWith(t.Name))).SingleInstance();

            builder.RegisterType<AccessTokenStore>().As<IAccessTokenStore>().SingleInstance();
            builder.RegisterType<MvxViewsContainerHelper>().As<IMvxViewsContainerHelper>().SingleInstance();
            builder.RegisterType<AkavacheContext>().As<IKeyValueStore>().SingleInstance();
            builder.RegisterType<ServiceSettings>().As<IServiceSettings>().SingleInstance();
            builder.RegisterType<AppSettings>().As<IStartable>().AsSelf().As<IAppSettings>().SingleInstance();
            builder.RegisterType<Now>().As<INow>().SingleInstance();

            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().SingleInstance();
        }
    }
}
