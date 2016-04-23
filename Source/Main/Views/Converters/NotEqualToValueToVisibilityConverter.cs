namespace Main.Views.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    // If the value is not equal to the parameter, returns visible, else hidden.
    public class NotEqualToValueToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() != parameter.ToString() ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}