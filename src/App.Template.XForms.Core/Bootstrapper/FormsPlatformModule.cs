using System.Linq;
using System.Reflection;
using App.Template.XForms.Core.MvvmCross;
using Autofac;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platform;

namespace App.Template.XForms.Core.Bootstrapper
{
    public class FormsPlatformModule : Autofac.Module
    {
        private const string ServicesEnding = "Service";
        private const string ModelsEnding = "Model";

        protected override void Load(ContainerBuilder builder)
        {
            // register any types that live in the platform PCL
            builder
                .RegisterType<MvxFormsViewLoader>()
                .As<IMvxFormsPageLoader>();

            builder.RegisterAssemblyTypes(CoreAssemblyHelper.CoreAssembly)
                .Where(t => t.GetTypeInfo().IsClass && t.Name.EndsWith(ServicesEnding))
                .As(t => t.GetInterfaces().Single(i => i.Name.EndsWith(t.Name))).SingleInstance();
        }
    }
}
