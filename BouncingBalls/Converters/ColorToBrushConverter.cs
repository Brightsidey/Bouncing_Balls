using System;
using System.Windows.Data;
using System.Windows.Media;

namespace BouncingBalls.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is Color color ? new SolidColorBrush(color) : Brushes.Transparent;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is SolidColorBrush brush ? brush.Color : Colors.Transparent;
        }
    }
}