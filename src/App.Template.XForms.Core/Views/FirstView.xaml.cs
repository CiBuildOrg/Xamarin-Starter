using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Template.XForms.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstView
    {
        public FirstView()
        {
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);
            //NavigationPage.SetHasBackButton(this, false);
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    return true;
        //}
    }
}