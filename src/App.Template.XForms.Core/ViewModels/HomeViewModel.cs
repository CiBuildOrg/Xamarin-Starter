using App.Template.XForms.Core.ViewModels.Base;
using MvvmCross.Core.Navigation;

namespace App.Template.XForms.Core.ViewModels
{
    public class HomeViewModel : BasePageViewModel
    {
        public void ScanBarcode()
        {
            ShowViewModel<ScanBarcodeViewModel>();
        }

        public HomeViewModel(IMvxNavigationService navigationService) : base(navigationService)
        {
        }
    }
}