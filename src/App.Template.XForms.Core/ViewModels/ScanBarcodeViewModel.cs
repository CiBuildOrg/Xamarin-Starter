﻿using System.Diagnostics.CodeAnalysis;
using App.Template.XForms.Core.ViewModels.Base;
using MvvmCross.Core.Navigation;

namespace App.Template.XForms.Core.ViewModels
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class ScanBarcodeViewModel : BaseMasterPageViewModel<MenuViewModel>
    {
        public ScanBarcodeViewModel(IMvxNavigationService navigationService) : base(navigationService)
        {
        }
    }
}
