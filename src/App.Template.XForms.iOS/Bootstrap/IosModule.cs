using App.Template.XForms.Core.Contracts;
using App.Template.XForms.iOS.Infrastructure.Interaction;
using App.Template.XForms.iOS.Infrastructure.Validation;
using Autofac;
using Xamarin.Auth;

namespace App.Template.XForms.iOS.Bootstrap
{
    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // do all IOS registrations here
            builder.Register(x => InteractiveAlerts.Instance).SingleInstance();

            builder.Register(ctx => AccountStore.Create());

            builder.RegisterType<MvxIosValidationService>().As<IMvxValidationService>();
        }
    }
}