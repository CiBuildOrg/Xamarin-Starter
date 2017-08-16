using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;

namespace App.Template.XForms.Android.Renderers
{
    internal class FontUtils
    {
        public static void ApplyTypeface(TextView view, string fontFamily)
        {
            if (string.IsNullOrEmpty(fontFamily)) return;

            var typeFace = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, fontFamily);

            if (typeFace != null)
            {
                view.Typeface = typeFace;
            }
        }
    }
}