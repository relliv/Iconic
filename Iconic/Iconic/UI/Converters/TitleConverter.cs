using System;
using System.Globalization;
using System.Threading;
using System.Windows.Data;

namespace Iconic.UI.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class TitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var title = (string)value;

            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(title.Replace("-", " ")).Replace("İ", "I");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}