using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeparationTrainer.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ContentProperty("ActionButtons")]
    public partial class ActionButton : ContentView
    {
        public static BindableProperty ActionButtonsProperty = BindableProperty.Create("ActionButtons",
            typeof(StackLayout),
            typeof(ContentView),
            default(StackLayout),
            BindingMode.TwoWay,
            propertyChanged: OnPropertyChanged);

        private static void OnPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (!(newvalue is StackLayout newStackLayout))
                throw new ArgumentException("Value passed must be a stack layout");

            var codeBehind = (ActionButton) bindable;
            
            codeBehind.ActionButtonContainer.Content = newStackLayout;
        }

        public StackLayout ActionButtons
        {
            get => (StackLayout) GetValue(ActionButtonsProperty);
            set => SetValue(ActionButtonsProperty, value);
        }

        public ActionButton()
        {
            InitializeComponent();
        }

        private async void ImageButton_OnClicked(object sender, EventArgs e)
        {
            ActionButtonContainer.IsVisible = !ActionButtonContainer.IsVisible;

            if(ActionButtonContainer.IsVisible)
            {
                await ActionButtonContainer.FadeTo(1.0, 500);
            }
            else
            {
                await ActionButtonContainer.FadeTo(0, 500);
            }
        }
    }
}