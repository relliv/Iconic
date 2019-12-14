using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Iconic.UI.Converters
{
    public class MultipleBindingFallbackConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var value = values.FirstOrDefault(x => x is string s ? !string.IsNullOrEmpty(s) : x != null && x != DependencyProperty.UnsetValue);

            //if (parameter != null && parameter is string param)
            //{
            //    if (param == "substring" && value is string valueString)
            //    {
            //        var maxLength = 150;

            //        return valueString.Length > maxLength ? valueString.Substring(0, maxLength) : valueString;
            //    }
            //}

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
