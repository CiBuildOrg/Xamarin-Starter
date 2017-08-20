using System;
using Android.Support.V7.App;
using App.Template.XForms.Core.Utils.Interaction;

namespace App.Template.XForms.Android.Infrastructure.Interaction
{
    public static class InteractiveAlerts
    {
        private static Lazy<IInteractiveAlerts> _instanceLazy =
                new Lazy<IInteractiveAlerts>(() => throw new ArgumentException(
                    "In android, you must call InteractiveAlerts.Init(Activity) from your first activity OR InteractiveAlerts.Init(App) from your custom application OR provide a factory function to get the current top activity via UserDialogs.Init(() => supply top activity)"))
            ;


        public static void Init(Func<AppCompatActivity> topActivityFunc)
        {
            _instanceLazy = new Lazy<IInteractiveAlerts>(() => new InteractiveAlertsImpl(topActivityFunc));
        }

        public static IInteractiveAlerts Instance => _instanceLazy.Value;
    }
}