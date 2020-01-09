using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Hurace.RaceControl.Converters
{
    public class SignedTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpanValue = (TimeSpan)value;

            if (timeSpanValue == TimeSpan.MaxValue)
                return "/";

            var prefix = "  ";
            if (timeSpanValue < TimeSpan.Zero)
                prefix = "- ";
            else if (timeSpanValue > TimeSpan.Zero)
                prefix = "+ ";

            return $"{prefix}{timeSpanValue.ToString("mm\\:ss\\.ff")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
