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

        public static BindableProperty TagModelProperty = BindableProperty.Create("TagModel",
            typeof(TagModel),
            typeof(ContentView),
            null,
            BindingMode.TwoWay,
            propertyChanged: ActivityTagModelPropertyChanged);

        public static BindableProperty RemoveTagCommandProperty = BindableProperty.Create("RemoveTagCommand",
            typeof(Command<TagModel>),
            typeof(ContentView),
            default(Command<TagModel>),
            BindingMode.TwoWay);

        public TagModel TagModel
        {
            get => (TagModel) GetValue(TagModelProperty);
            set => SetValue(TagModelProperty, value);
        }

        public Command<TagModel> RemoveTagCommand
        {
            get => (Command<TagModel>) GetValue(RemoveTagCommandProperty);
            set => SetValue(RemoveTagCommandProperty, value);
        }

        public static void ActivityTagModelPropertyChanged(BindableObject bindableObject, object oldValue,
            object newValue)
        {

        }
    }
}