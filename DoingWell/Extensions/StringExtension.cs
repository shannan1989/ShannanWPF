using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Shannan.DoingWell
{
    public static class StringExtension
    {
        public static double MeasureTextWidth(this string self, double fontSize, string fontFamily)
        {
            FormattedText formattedText = new FormattedText(self, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(fontFamily), fontSize, Brushes.Black);
            return formattedText.WidthIncludingTrailingWhitespace;
        }
    }
}
