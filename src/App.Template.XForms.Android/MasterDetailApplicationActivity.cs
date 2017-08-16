using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Platform;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Droid;
using MvvmCross.Forms.Droid.Presenters;
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

            var mvxFormsApp = new MvxFormsApplication();
            LoadApplication(mvxFormsApp);
            //var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsDroidPagePresenter;
            if (Mvx.Resolve<IMvxViewPresenter>() is MvxFormsDroidMasterDetailPagePresenter presenter)
                presenter.FormsApplication = mvxFormsApp;

            Mvx.Resolve<IMvxAppStart>().Start();
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