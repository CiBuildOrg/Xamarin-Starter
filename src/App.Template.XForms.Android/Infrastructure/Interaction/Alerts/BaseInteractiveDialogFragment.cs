using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using App.Template.XForms.Android.Infrastructure.Interaction.TopViewHolders;
using App.Template.XForms.Core.Utils.Interaction;
using AlertDialog = Android.App.AlertDialog;

namespace App.Template.XForms.Android.Infrastructure.Interaction.Alerts
{
    public class BaseInteractiveDialogFragment<TConfig> : AppCompatDialogFragment where TConfig : InteractiveAlertConfig
    {
        protected TConfig Config { get; set; }

        private ITopContentViewHolder _topViewHolder;

        protected BaseInteractiveDialogFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        protected BaseInteractiveDialogFragment()
        {

        }

        public override void OnStart()
        {
            base.OnStart();
            _topViewHolder?.OnStart();
        }

        public override void OnPause()
        {
            base.OnPause();
            _topViewHolder?.OnPause();
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var alertDialogBuilder = new AlertDialog.Builder(Activity);
            var cancelConfig = Config.CancelButton;
            if (cancelConfig != null)
            {
                var cancelTitle = cancelConfig.Title ?? InteractiveAlertConfig.DefaultCancelText;
                alertDialogBuilder.SetNegativeButton(cancelTitle, (sender, e) =>
                {
                    var handler = cancelConfig.Action;
                    if (handler != null)
                    {
                        handler();
                    }
                    else
                    {
                        Dismiss();
                    }
                });
            }

            var okConfig = Config.OkButton;
            if (okConfig != null)
            {
                var okTitle = okConfig.Title ?? InteractiveAlertConfig.DefaultOkText;
                alertDialogBuilder.SetPositiveButton(okTitle, (sender, e) =>
                {
                    var handler = okConfig.Action;
                    if (handler != null)
                    {
                        handler();
                    }
                    else
                    {
                        Dismiss();
                    }
                });
            }

            var contentView = (LinearLayout)LayoutInflater.From(Context).Inflate(Resource.Layout.alert_dialog, null);
            var bottomView = contentView.FindViewById<LinearLayout>(Resource.Id.alert_dialog_bottom);
            OnSetContentView(contentView);

            // try set bottom view
            bottomView.Visibility = OnSetBottomView(bottomView) ? ViewStates.Visible : ViewStates.Gone;

            var topContentView = contentView.FindViewById<FrameLayout>(Resource.Id.alert_dialog_top);
            _topViewHolder = TopContentFactory.CreateTopViewHolder(Context, topContentView, Config.Style);
            _topViewHolder.ContentView.RequestLayout();

            // set text
            SetContentText(contentView, Resource.Id.alert_dialog_title, Config.Title);
            SetContentText(contentView, Resource.Id.alert_dialog_content, Config.Message);
            alertDialogBuilder.SetView(contentView);

            return alertDialogBuilder.Create();
        }

        protected void SetContentText(View contentView, int textViewId, string text)
        {
            var textView = contentView.FindViewById<TextView>(textViewId);
            if (string.IsNullOrEmpty(text))
            {
                textView.Visibility = ViewStates.Gone;
            }
            else
            {
                textView.Text = text;
            }
        }

        protected virtual void OnSetContentView(ViewGroup viewGroup)
        {

        }

        protected virtual bool OnSetBottomView(ViewGroup viewGroup)
        {
            return false;
        }

        public static T NewInstance<T>(TConfig alertConfig) where T : BaseInteractiveDialogFragment<TConfig>
        {
            var dialogFragment = (T)Activator.CreateInstance(typeof(T));
            dialogFragment.Config = alertConfig;

            return dialogFragment;
        }
    }
}