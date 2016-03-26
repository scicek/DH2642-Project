namespace Main.Views.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;
    using Models;

    public class ActivityTypeToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var activityType = (ActivityType)value;

            switch (activityType)
            {
                case ActivityType.Presentation:
                    return new SolidColorBrush(Colors.DodgerBlue);
                case ActivityType.GroupWork:
                    return new SolidColorBrush(Colors.IndianRed);
                case ActivityType.Discussion:
                    return new SolidColorBrush(Colors.ForestGreen);
                case ActivityType.Break:
                    return new SolidColorBrush(Colors.Goldenrod);
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
