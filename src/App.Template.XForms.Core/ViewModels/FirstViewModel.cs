using System;
using System.Diagnostics.CodeAnalysis;
using App.Template.XForms.Core.Utils.Interaction;
using MvvmCross.Core.ViewModels;

namespace App.Template.XForms.Core.ViewModels
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class FirstViewModel : MvxViewModel
    {
        private readonly IInteractiveAlerts _alerts;
        private static int _ctorCount;

        public int CtorCount => _ctorCount;

        public FirstViewModel(IInteractiveAlerts alerts)
        {
            _alerts = alerts;
            _ctorCount++;
        }

        private MvxCommand _resetCounter;
        public MvxCommand ResetCounter => _resetCounter ?? (_resetCounter = new MvxCommand(ResetCounterImplementation));
        public class AlertConfigItem
        {
            public string Title { get; set; }

            public Action Command { get; set; }
        }

        protected void CreateAlertConfigItem(string title, string alertMessage, InteractiveAlertStyle style)
        {
            var alertConfig = new InteractiveAlertConfig
            {
                OkButton = new InteractiveActionButton(),
                CancelButton = new InteractiveActionButton(),
                Message = alertMessage,
                Title = title,
                Style = style,
                IsCancellable = true
            };

            _alerts.ShowAlert(alertConfig);
        }
        public void ResetCounterImplementation()
        {
            _ctorCount = 0;
            RaisePropertyChanged(nameof(CtorCount));
            CreateAlertConfigItem("Success", "Reset was OK", InteractiveAlertStyle.Warning);
        }
    }
}