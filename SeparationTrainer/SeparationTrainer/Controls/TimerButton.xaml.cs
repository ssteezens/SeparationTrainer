using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeparationTrainer.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimerButton : ContentView
    {
        private string _timerText;

        public TimerButton()
        {
            InitializeComponent();
        }

        public static BindableProperty ElapsedTimeProperty = BindableProperty.Create("ElapsedTimed",
            typeof(TimeSpan),
            typeof(ContentView),
            TimeSpan.MinValue,
            BindingMode.TwoWay,
            propertyChanged: ElapsedTimePropertyChanged);

        public static BindableProperty ButtonCommandProperty = BindableProperty.Create("ButtonCommand",
            typeof(Command),
            typeof(ContentView),
            default(Command),
            BindingMode.TwoWay,
            propertyChanged: ElapsedTimePropertyChanged);

        public TimeSpan ElapsedTime
        {
            get => (TimeSpan)GetValue(ElapsedTimeProperty);
            set => SetValue(ElapsedTimeProperty, value);
        }

        public Command ButtonCommand
        {
            get => (Command)GetValue(ButtonCommandProperty);
            set => SetValue(ButtonCommandProperty, value);
        }

        public static void ElapsedTimePropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var codeBehind = (TimerButton) bindableObject;

            var hoursText = codeBehind.ElapsedTime.Hours > 0 ?
                $"{codeBehind.ElapsedTime.Hours.ToString().PadLeft(2, '0')}:"
                : string.Empty;
            var minutesText = codeBehind.ElapsedTime.Minutes > 0 ?
                $"{codeBehind.ElapsedTime.Minutes.ToString().PadLeft(2, '0')}:"
                : string.Empty;
            var secondsText = codeBehind.ElapsedTime.Seconds > 0 ?
                $"{codeBehind.ElapsedTime.Seconds.ToString().PadLeft(2, '0')}:"
                : "00:";
            var hundredths = (int)Math.Round((double)codeBehind.ElapsedTime.Milliseconds / 10, 2);
            var millisecondsText = codeBehind.ElapsedTime.Milliseconds > 0 ? 
                hundredths.ToString().PadLeft(2, '0')
                : "00";

            codeBehind.TimerText = $"{hoursText}{minutesText}{secondsText}{millisecondsText}";
        }

        public string TimerText
        {
            get => _timerText;
            set
            {
                _timerText = value;
                OnPropertyChanged(nameof(TimerText));
            }
        }
    }
}