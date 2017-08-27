using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using App.Template.XForms.Core.ViewModels.Base;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Views;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Presenters;
using MvvmCross.Forms.ViewModels;
using MvvmCross.Platform;
using Xamarin.Forms;

namespace App.Template.XForms.Android.Presenters
{
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    public class CustomPresenter : MvxViewPresenter, IMvxFormsPagePresenter, IMvxAndroidViewPresenter
    {
        private const string ModalPresentationParameter = "modal";

        public override void Show(MvxViewModelRequest request)
        {
            var viewModelType = request.ViewModelType;
            var isMasterViewModel = viewModelType.GetCustomAttribute<MasterPageViewModelAttribute>() != null;
            var isPageViewModel = viewModelType.GetCustomAttribute<PageViewModelAttribute>() != null;

            if (isPageViewModel)
            {
                if (TryShowPage(request))
                    return;

                Mvx.Error("Skipping request for {0}", request.ViewModelType.Name);
            }
            else if (isMasterViewModel)
            {
                if (TryShowMasterDetailPage(request))
                    return;

                Mvx.Error("Skipping request for {0}", request.ViewModelType.Name);
            }
            else throw new Exception("VM not page or master detail ");
        }

        protected virtual void CustomPlatformInitialization(MasterDetailPage mainPage)
        {
        }


        protected virtual void CustomPlatformInitialization(NavigationPage mainPage)
        {
        }

        private static void SetupPageForBinding(BindableObject page, IMvxViewModel viewModel, MvxViewModelRequest request)
        {
            if (page is IMvxContentPage contentPage)
            {
                contentPage.Request = request;
                contentPage.ViewModel = viewModel;
            }
            else
            {
                page.BindingContext = viewModel;
            }
        }

        private bool TryShowPage(MvxViewModelRequest request)
        {
            var page = MvxPresenterHelpers.CreatePage(request);
            if (page == null)
                return false;

            var viewModel = MvxPresenterHelpers.LoadViewModel(request);

            SetupPageForBinding(page, viewModel, request);

            var mainPage = FormsApplication.MainPage as NavigationPage;

            if (mainPage == null)
            {
                FormsApplication.MainPage = new NavigationPage(page);
                mainPage = (NavigationPage) FormsApplication.MainPage;
                CustomPlatformInitialization(mainPage);
            }
            else
            {
                try
                {
                    // check for modal presentation parameter
                    if (request.PresentationValues != null && request.PresentationValues.TryGetValue(ModalPresentationParameter, out string modalParameter) && bool.Parse(modalParameter))
                        mainPage.Navigation.PushModalAsync(page);
                    else
                        // calling this sync blocks UI and never navigates hence code continues regardless here
                        mainPage.PushAsync(page);
                }
                catch (Exception e)
                {
                    Mvx.Error("Exception pushing {0}: {1}\n{2}", page.GetType(), e.Message, e.StackTrace);
                    return false;
                }
            }

            return true;
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint presentationHint)
            {
                var closePresentationHint = presentationHint;

                var viewModelType = closePresentationHint.ViewModelToClose.GetType();
                var isMasterViewModel = viewModelType.GetCustomAttribute<MasterPageViewModelAttribute>() != null;
                var isPageViewModel = viewModelType.GetCustomAttribute<PageViewModelAttribute>() != null;

                if (isPageViewModel)
                {
                    ChangePagePresentation();
                }
                else if (isMasterViewModel)
                {
                    ChangeMasterDetailPresentation();
                }
                else throw new Exception("VM not page or master detail ");
            }
        }

        public override void Close(IMvxViewModel toClose)
        {
            // do nothing
        }

        private static void SetupMasterDetailForBinding(BindableObject page, IMvxViewModel viewModel, MvxViewModelRequest request)
        {
            if (page is IMvxContentPage contentPage)
            {
                contentPage.Request = request;
                contentPage.ViewModel = viewModel;
            }
            else
            {
                page.BindingContext = viewModel;
            }
        }

        private void ChangePagePresentation()
        {
            var mainPage = FormsApplication.MainPage as NavigationPage;

            if (mainPage == null)
            {
                Mvx.TaggedTrace("MvxFormsPresenter:ChangePresentation()", "Oops! Don't know what to do");
            }
            else
            {
                mainPage.PopAsync();
            }
        }

        private void ChangeMasterDetailPresentation()
        {
            var mainPage = FormsApplication.MainPage as MasterDetailPage;

            if (mainPage == null)
            {
                Mvx.TaggedTrace("MvxFormsPresenter:ChangePresentation()", "Oops! Don't know what to do");
            }
            else
            {
                // Perform pop on the Detail Page and launch RootContentPageActivated if root has been reached
                var navPage = mainPage.Detail as NavigationPage;
                if (navPage == null) return;
                navPage.PopAsync();
                if (navPage.Navigation.NavigationStack.Count == 1)
                    MasterRootContentPageActivated();
            }
        }

        private bool TryShowMasterDetailPage(MvxViewModelRequest request)
        {
            var page = MvxPresenterHelpers.CreatePage(request);
            if (page == null)
                return false;

            var viewModel = MvxPresenterHelpers.LoadViewModel(request);

            SetupMasterDetailForBinding(page, viewModel, request);

            var mainPage = FormsApplication.MainPage as MasterDetailPage;

            // Initialize the MasterDetailPage container            
            if (mainPage == null)
            {
                // The ViewModel used should inherit from MvxMasterDetailViewModel, so we can create a new ContentPage for use in the Detail page
                var masterDetailViewModel = viewModel as MvxMasterDetailViewModel;
                if (masterDetailViewModel == null)
                    throw new InvalidOperationException("ViewModel should inherit from MvxMasterDetailViewModel<T>");

                Page rootContentPage;
                if (masterDetailViewModel.RootContentPageViewModelType != null)
                {
                    var rootContentRequest = new MvxViewModelRequest(masterDetailViewModel.RootContentPageViewModelType, null, null);

                    var rootContentViewModel = MvxPresenterHelpers.LoadViewModel(rootContentRequest);
                    rootContentPage = MvxPresenterHelpers.CreatePage(rootContentRequest);
                    SetupMasterDetailForBinding(rootContentPage, rootContentViewModel, rootContentRequest);
                }
                else
                    rootContentPage = new ContentPage();

                var navPage = new NavigationPage(rootContentPage);

                //Hook to Popped event to launch RootContentPageActivated if proceeds
                navPage.Popped += (sender, e) =>
                {
                    if (navPage.Navigation.NavigationStack.Count == 1)
                        MasterRootContentPageActivated();
                };

                mainPage = new MasterDetailPage
                {
                    Master = page,
                    Detail = navPage
                };

                FormsApplication.MainPage = mainPage;
                CustomPlatformInitialization(mainPage);
            }
            else
            {
                // Functionality for clearing the navigation stack before pushing to new Page (for example in a menu with multiple options)
                if (request.PresentationValues != null)
                {
                    if (request.PresentationValues.ContainsKey("NavigationMode") && request.PresentationValues["NavigationMode"] == "ClearStack")
                    {
                        mainPage.Detail.Navigation.PopToRootAsync();
                        if (Device.Idiom == TargetIdiom.Phone)
                            mainPage.IsPresented = false;
                    }
                }

                try
                {
                    var nav = mainPage.Detail as NavigationPage;

                    // calling this sync blocks UI and never navigates hence code continues regardless here
                    nav?.PushAsync(page);
                }
                catch (Exception e)
                {
                    Mvx.Error("Exception pushing {0}: {1}\n{2}", page.GetType(), e.Message, e.StackTrace);
                    return false;
                }
            }

            return true;
        }

        private static void MasterRootContentPageActivated()
        {
            var mainPage = Application.Current.MainPage as MasterDetailPage;
            (mainPage?.Master.BindingContext as MvxMasterDetailViewModel)?.RootContentPageActivated();
        }

        public CustomPresenter(MvxFormsApplication formsApplication)
        {   
            FormsApplication = formsApplication
                ?? throw new ArgumentNullException(nameof(formsApplication), "MvxFormsApp cannot be null");
        }

        public MvxFormsApplication FormsApplication { get; set; }
    }   
}