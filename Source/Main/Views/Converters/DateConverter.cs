namespace Main.Views.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using Models;

    // Gets the day, month or year of a date, depending on the parameter.
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (Date) value;

            var param = (string) parameter;

            switch(param.ToLowerInvariant())
            {
                case "day":
                    return date.Day;
                case "month":
                    return date.Month;
                case "year":
                    return date.Year;
                default:
                    return date.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}