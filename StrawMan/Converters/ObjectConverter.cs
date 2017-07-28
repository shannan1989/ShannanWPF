using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Shannan.StrawMan.Converters
{
    internal sealed class ObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] param = parameter.ToString().ToLower().Split(':');
            if (value == null)
            {
                return param[2];
            }
            if (param[0].Contains("|"))
            {
                return param[0].Split('|').Contains(value.ToString().ToLower()) ? param[1] : param[2];
            }
            return param[0].Equals(value.ToString().ToLower()) ? param[1] : param[2];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
