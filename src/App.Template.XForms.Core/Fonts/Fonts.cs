using Xamarin.Forms;

namespace App.Template.XForms.Core.Fonts
{
    public static class Fonts
    {
        public static string FontAwesome
        {
            get
            {
                var fontAwesomeName = "";
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        fontAwesomeName = "fontawesome.ttf";
                        break;
                    case Device.Android:
                        fontAwesomeName = "fontawesome.ttf";
                        break;
                    case Device.WinPhone:
                        fontAwesomeName = "fontawesome.ttf";
                        break;
                }

                return fontAwesomeName;
            }
        }
    }
}
