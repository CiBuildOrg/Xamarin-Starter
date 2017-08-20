using App.Template.XForms.iOS.Infrastructure.Interaction;
using Autofac;

namespace App.Template.XForms.iOS.Bootstrap
{
    public class IosModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // do all IOS registrations here
            builder.Register(x => InteractiveAlerts.Instance).SingleInstance();
        }
    }
}