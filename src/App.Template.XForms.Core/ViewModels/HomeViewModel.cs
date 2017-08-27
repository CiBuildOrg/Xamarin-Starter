using System.Diagnostics.CodeAnalysis;
using App.Template.XForms.Core.ViewModels.Base;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace App.Template.XForms.Core.ViewModels
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class HomeViewModel : BasePageViewModel
    {
        public HomeViewModel(IMvxNavigationService navigationService) : base(navigationService)
        {

        }

        private IMvxCommand _scanBarcodeCommand;

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public IMvxCommand ScanBarcode => _scanBarcodeCommand ??
                                          (_scanBarcodeCommand = new MvxCommand(ScanBarcodeImplementation));

        private void ScanBarcodeImplementation()
        {
            ShowViewModel<ScanBarcodeViewModel>();
        }
    }
}