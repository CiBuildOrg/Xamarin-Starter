using System;
using App.Template.XForms.Core.Utils.Interaction;

namespace App.Template.XForms.iOS.Infrastructure.Interaction
{
    public static class InteractiveAlerts
    {
        private static readonly Lazy<IInteractiveAlerts> InstanceLazy = 
            new Lazy<IInteractiveAlerts>(() => new InteractiveAlertsImpl());

        public static IInteractiveAlerts Instance => InstanceLazy.Value;
    }
}