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
    public partial class ActivityGraphPage : ContentPage
    {
        public ActivityGraphPage()
        {
            InitializeComponent();

            ViewModel = new ActivityGraphViewModel();
            BindingContext = ViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            IsBusy = true;

            await ViewModel.OnAppearing();

            IsBusy = false;
        }

        public ActivityGraphViewModel ViewModel { get; set; }
    }
}