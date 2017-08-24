using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;

namespace App.Template.XForms.Core.Contracts
{
    public interface IMvxValidationService
    {
        void SetupForValidation(IMvxBindingContext context, IMvxViewModel viewModel);
    }
}