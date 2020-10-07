using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Helpers;
using Xamarin.Forms;

namespace SeparationTrainer.Converters
{
    public class BoolToStartStopGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolValue))
                throw new ArgumentException("Value passed to converter must be a bool");

            return boolValue ? IconFont.Pause : IconFont.Play;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
