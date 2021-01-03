using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeparationTrainer.Controls
{
    public class ThemeEditor : Editor
    {
        public static readonly BindableProperty BottomLineColorProperty = BindableProperty.Create("BottomLineColor", typeof(Color), typeof(ThemeEditor), Color.Black, BindingMode.TwoWay,  propertyChanged:
            (bindable, value, newValue) =>
            {
                var editor = (ThemeEditor) bindable;
                editor.BottomLineColor = (Color)newValue;
            });

        public Color BottomLineColor
        {
            get => (Color)GetValue(BottomLineColorProperty);
            set => SetValue(BottomLineColorProperty, value);
        }
    }
}
