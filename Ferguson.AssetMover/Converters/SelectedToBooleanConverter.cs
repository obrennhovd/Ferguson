using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Ferguson.AssetMover.Converters
{
    public class SelectedToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value > -1) return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType,  object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
