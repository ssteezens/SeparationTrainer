using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Models;
using SeparationTrainer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeparationTrainer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewSessionsPage : ContentPage
    {
        public ViewSessionsPage()
        {
            InitializeComponent();

            ViewModel = new ViewSessionsViewModel();
            BindingContext = ViewModel;

            SessionsCarouselView.IsScrollAnimated = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            IsBusy = true;

            await ViewModel.OnAppearing();

            IsBusy = false;
        }

        public ViewSessionsViewModel ViewModel { get; set; }
    }
}