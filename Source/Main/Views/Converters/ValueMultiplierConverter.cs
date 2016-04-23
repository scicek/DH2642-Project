namespace Main.Views.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    // Multiplies two arguments.
    public class ValueMultiplierConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)values[1] * (double)values[0];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
