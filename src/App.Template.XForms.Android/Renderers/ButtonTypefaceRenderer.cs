using App.Template.XForms.Android.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(Button), typeof(ButtonTypefaceRenderer))]

namespace App.Template.XForms.Android.Renderers
{
    public class ButtonTypefaceRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            FontUtils.ApplyTypeface(Control, Element.FontFamily);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            FontUtils.ApplyTypeface(Control, Element.FontFamily);
        }
    }
}