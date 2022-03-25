using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeparationTrainer.Converters
{
    public class XAxisLabelHorizontalOptionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return LayoutOptions.FillAndExpand;
            if (!(value is string stringValue))
                throw new ArgumentException("Value passed to converter must be a string");
            if (!(parameter is Views.ActivityGraphPage page))
                throw new ArgumentException("Parameter passed to converter must be the ActivityGraphPage");

            // Xamarin.Forms doesn't support Bindable properties in converter parameter WHYYYY!!??
            var labelCollection = page.ViewModel.XAxisLabels;
            var index = labelCollection.IndexOf(stringValue);
            var isFirstElement = index == 0;
            var isLastElement = index == labelCollection.Count - 1;

            if (isFirstElement)
                return LayoutOptions.StartAndExpand;
            else if (isLastElement)
                return LayoutOptions.EndAndExpand;
            else
                return LayoutOptions.FillAndExpand;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
