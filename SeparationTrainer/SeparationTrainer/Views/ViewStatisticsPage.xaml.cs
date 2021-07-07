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
    public partial class ViewStatisticsPage : ContentPage
    {
        public ViewStatisticsPage()
        {
            InitializeComponent();

            ViewModel = new ViewStatisticsViewModel();
            BindingContext = ViewModel;
        }

        protected override async void OnAppearing()
        {
            await ViewModel.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            Shell.Current.GoToAsync($"//{nameof(ViewSessionsPage)}").Wait();

            return true;
        }

        public ViewStatisticsViewModel ViewModel { get; set; }
    }
}