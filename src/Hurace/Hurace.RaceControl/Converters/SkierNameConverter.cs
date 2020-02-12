using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Hurace.RaceControl.Converters
{
    class SkierNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Domain.Skier skierValue)
            {
                return $"{skierValue.LastName} {skierValue.FirstName}";
            }
            else
            {
                throw new InvalidOperationException($"Passed value is not of dynamic type '{nameof(Domain.Skier)}' but '{value.GetType().Name}'");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
