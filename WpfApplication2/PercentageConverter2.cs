using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfApplication2
{
    public class PercentageConverter2 : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(values[0])*
                   System.Convert.ToDouble(parameter);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}