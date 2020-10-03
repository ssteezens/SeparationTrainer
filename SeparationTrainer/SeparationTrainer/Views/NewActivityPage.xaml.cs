using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeparationTrainer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewActivityPage : ContentPage
    {
        public NewActivityPage()
        {
            InitializeComponent();
            ViewModel = new NewActivityViewModel();

            this.BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel?.Refresh();
        }

        public NewActivityViewModel ViewModel { get; set; }
    }
}