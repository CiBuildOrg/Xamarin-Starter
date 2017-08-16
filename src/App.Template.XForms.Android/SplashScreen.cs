using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Xamarin.Forms;

namespace App.Template.XForms.Android
{
    [Activity(
        Label = "App.Template"
        , MainLauncher = true
        , Icon = "@mipmap/ic_launcher"
        , Theme = "@style/Theme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen() : base(Resource.Layout.SplashScreen)
        {
        }

        private bool _isInitializationComplete;

        public override void InitializationComplete()
        {
            if (_isInitializationComplete) return;
            _isInitializationComplete = true;
            StartActivity(typeof(MasterDetailApplicationActivity));
        }

        protected override void OnCreate(Bundle bundle)
        {
            Forms.Init(this, bundle);
            // Leverage controls' StyleId attrib. to Xamarin.UITest
            Forms.ViewInitialized += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.View.StyleId))
                {
                    e.NativeView.ContentDescription = e.View.StyleId;
                }
            };

            base.OnCreate(bundle);

            
        }
    }
}