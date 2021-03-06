﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using App.Template.XForms.Android.Infrastructure.Interaction;
using App.Template.XForms.Android.Presenters;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Platform;
using MvvmCross.Forms.Core;
using MvvmCross.Platform;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace App.Template.XForms.Android
{
    [Activity(Label = "App.Template", MainLauncher = false, Icon = "@mipmap/ic_launcher", Theme = "@style/AppTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MasterDetailApplicationActivity : FormsAppCompatActivity
    {
        private IMvxAndroidActivityLifetimeListener _lifetimeListener;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.tabbar;
            ToolbarResource = Resource.Layout.toolbar;
            
            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext).EnsureInitialized();

            // register interactive alerts
            InteractiveAlerts.Init(() => this);
            Mvx.RegisterSingleton(InteractiveAlerts.Instance);

            var mvxFormsApp = new MvxFormsApplication();
            LoadApplication(mvxFormsApp);
            if (Mvx.Resolve<IMvxViewPresenter>() is CustomPresenter presenter)
            {
                presenter.FormsApplication = mvxFormsApp;
            }

            Mvx.Resolve<IMvxAppStart>().Start();

            //var cancellationToken = CancellationToken.None;
            //var autheService = Mvx.Resolve<IAuthenticationService>();
            //var needsToAuthenticate = autheService.NeedsToAuthenticate(cancellationToken).WaitAsync().Result;
            //if (needsToAuthenticate)
            //{
            //    try
            //    {
            //        var token = autheService.GetAccessToken("adam", "asdf3235", cancellationToken).WaitAsync().Result;
            //        var accessToken = token.Token;
            //    }
            //    catch (Exception ex)
            //    {
                    
            //    }
            //}

            _lifetimeListener = Mvx.Resolve<IMvxAndroidActivityLifetimeListener>();
            _lifetimeListener.OnCreate(this);
        }

        protected override void OnDestroy()
        {
            _lifetimeListener.OnDestroy(this);
            base.OnDestroy();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            _lifetimeListener.OnViewNewIntent(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            _lifetimeListener.OnResume(this);
        }

        protected override void OnPause()
        {
            _lifetimeListener.OnPause(this);
            base.OnPause();
        }

        protected override void OnStart()
        {
            base.OnStart();
            _lifetimeListener.OnStart(this);
        }

        protected override void OnRestart()
        {
            base.OnRestart();
            _lifetimeListener.OnRestart(this);
        }

        protected override void OnStop()
        {
            _lifetimeListener.OnStop(this);
            base.OnStop();
        }
    }
}