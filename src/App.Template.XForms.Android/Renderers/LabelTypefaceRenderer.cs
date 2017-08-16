using App.Template.XForms.Android.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(LabelTypefaceRenderer))]
namespace App.Template.XForms.Android.Renderers
{

    public class LabelTypefaceRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            FontUtils.ApplyTypeface(Control, Element.FontFamily);
        }
    }
}