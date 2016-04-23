namespace Main.Views.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Data;
    using Models;

    // Given a dictionary and a date, retrieves the day.
    public class DateToDayConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (values != null && values.Length >= 2)
                {
                    var dict = values[0] as IDictionary<Date, Day>;
                    var date = (Date)values[1];

                    if (dict != null && dict.ContainsKey(date))
                    {
                        return dict[date];
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
