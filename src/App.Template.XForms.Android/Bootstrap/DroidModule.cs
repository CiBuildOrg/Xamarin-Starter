using App.Template.XForms.Core.Contracts;
using Autofac;
using Xamarin.Auth;

namespace App.Template.XForms.Android.Bootstrap
{
    public class DroidModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // do all Android registrations here

            builder.Register(ctx =>
            {
                //var password = ctx.Resolve<IAppSettings>().Security.StorePassword;
                return AccountStore.Create();
            });
        }
    }
}