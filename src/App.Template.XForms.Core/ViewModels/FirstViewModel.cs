using System.Diagnostics.CodeAnalysis;
using App.Template.XForms.Core.Utils.Interaction;
using App.Template.XForms.Core.ViewModels.Base;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace App.Template.XForms.Core.ViewModels
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class FirstViewModel : BasePageViewModel
    {
        private readonly IInteractiveAlerts _alerts;
        private static int _ctorCount;

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
        public int CtorCount => _ctorCount;

        public FirstViewModel(IInteractiveAlerts alerts, IMvxNavigationService navigationService) : base(navigationService)
        {
            _alerts = alerts;
            _ctorCount++;
        }

        private MvxCommand _resetCounter;
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public MvxCommand ResetCounter => _resetCounter ?? (_resetCounter = new MvxCommand(ResetCounterImplementation));


        private void CreateAlertConfigItem(string title, string alertMessage, InteractiveAlertStyle style)
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

        private void ResetCounterImplementation()
        {
            _ctorCount = 0;
            RaisePropertyChanged(nameof(CtorCount));
            CreateAlertConfigItem("Success", "Reset was OK", InteractiveAlertStyle.Warning);
        }
    }
}