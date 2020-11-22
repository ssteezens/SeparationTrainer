using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeparationTrainer.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TimeSpan timeSpanValue))
                throw new ArgumentException("Value passed to converter must be a TimeSpan");

            var hoursText = timeSpanValue.Hours > 0 ? $"{timeSpanValue.Hours} h " : "";
            var minutesText = timeSpanValue.Minutes > 0 ? $"{timeSpanValue.Minutes} m " : "";
            var secondsText = timeSpanValue.Seconds > 0 ? $"{timeSpanValue.Seconds} s" : "";

            var stringValue = $"{hoursText}{minutesText}{secondsText}";

            return stringValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
