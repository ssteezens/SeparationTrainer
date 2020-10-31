using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeparationTrainer.Controls
{
    public class IntegerSlider : Slider
    {
        public IntegerSlider()
        {
            this.ValueChanged += OnOnValueChanged;
        }

        private void OnOnValueChanged(object sender, ValueChangedEventArgs e)
        {
            var step = Math.Round(e.NewValue / 1);

            Value = step;
        }
    }
}
