using System;
using System.Diagnostics.CodeAnalysis;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Presenters;

namespace App.Template.XForms.Core.MvvmCross
{
    [SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
    public class MvxFormsViewLoader : MvxFormsPageLoader, IMvxFormsPageLoader
    {
        protected override string GetPageName(MvxViewModelRequest request)
        {
            var viewModelName = request.ViewModelType.Name;
            return viewModelName.Replace(MvvmConfig.ViewModelSuffix, MvvmConfig.ViewSuffix);
        }
    }   
}