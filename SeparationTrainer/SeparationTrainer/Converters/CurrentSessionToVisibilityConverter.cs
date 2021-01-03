using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Models;
using Xamarin.Forms;

namespace SeparationTrainer.Converters
{
    public class CurrentSessionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            if(!(value is SessionModel session))
                throw new ArgumentException("Value passed to converter must be a session");

            return session.Created.Date != DateTime.Today;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
