using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App.Template.XForms.Core.Utils.Interaction;

namespace App.Template.XForms.Android.Infrastructure.Interaction.Alerts
{
    public class EditableInteractiveDialogFragment : BaseInteractiveDialogFragment<EditableInteractiveAlertConfig>
    {
        protected EditableInteractiveDialogFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public EditableInteractiveDialogFragment()
        {

        }

        protected override bool OnSetBottomView(ViewGroup viewGroup)
        {
            var editText = new EditText(Context);
            editText.SetMaxLines(Config.SingleLine ? 1 : 10);
            editText.SetSingleLine(Config.SingleLine);
            editText.Hint = Config.Placeholder;
            var lp = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            editText.LayoutParameters = lp;
            editText.TextChanged += (s, e) =>
            {
                Config.Text = e.Text?.ToString();
            };

            viewGroup.AddView(editText);
            viewGroup.Invalidate();

            return true;
        }
    }
}