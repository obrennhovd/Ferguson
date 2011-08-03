﻿using System;
using System.Windows.Data;
using System.Globalization;

namespace Ferguson.AssetMover.Client.Converters
{
    public class HasItemsToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value > 0) return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType,  object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
