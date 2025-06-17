using System;
using System.Windows.Data;

namespace BouncingBalls.Converters
{
    public class DoubleToDoubleConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is double d ? d * 2 : 0;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is double d ? d / 2 : 0;
        }
    }
}