﻿namespace Main.Views.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    // Multiplies two values and returns it as a thickness with 'Left' property set to the value.
    public class MinimumBreakToMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness((double) values[1] * (double) values[0], 0, 0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
