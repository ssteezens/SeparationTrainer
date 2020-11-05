using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeparationTrainer.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TagControl : ContentView
    {
        public TagControl()
        {
            InitializeComponent();
        }

        public static BindableProperty ActivityTagModelProperty = BindableProperty.Create("TagModel",
            typeof(TagModel),
            typeof(ContentView),
            TimeSpan.MinValue,
            BindingMode.TwoWay,
            propertyChanged: ActivityTagModelPropertyChanged);

        public TagModel TagModel
        {
            get => (TagModel) GetValue(ActivityTagModelProperty);
            set => SetValue(ActivityTagModelProperty, value);
        }

        public static void ActivityTagModelPropertyChanged(BindableObject bindableObject, object oldValue,
            object newValue)
        {

        }
    }
}