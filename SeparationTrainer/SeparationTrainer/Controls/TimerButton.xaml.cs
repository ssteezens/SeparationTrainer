using SeparationTrainer.Extensions;
using System;
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

            codeBehind.TimerText = codeBehind.ElapsedTime.ToStopwatchForm();
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