using System;
using System.Globalization;
using System.Windows.Data;

namespace Shannan.StrawMan.Converters
{
    internal sealed class PercentToAngleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return 360 * (double)values[0] / (double)values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
