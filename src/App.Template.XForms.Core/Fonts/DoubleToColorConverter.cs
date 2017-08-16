using System;
using Xamarin.Forms;

namespace App.Template.XForms.Core.Fonts
{
    public class DoubleToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var num = (double)value;
            return new Color((num - 1.0) * -1.0, 1.0 - num, num);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}