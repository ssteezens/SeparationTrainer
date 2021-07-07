using System;
using SeparationTrainer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeparationTrainer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("SelectedDate", "selectedDate")]
    public partial class NewManualActivityPage : ContentPage
    {
        private string _selectedDate;
        private NewManualActivityViewModel _viewModel;

        public NewManualActivityPage()
        {
            InitializeComponent();
            ViewModel = new NewManualActivityViewModel();

            this.BindingContext = ViewModel;
        }

        public string SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;

                var date = DateTime.Parse(SelectedDate);
                ViewModel = new NewManualActivityViewModel() {SelectedDate = date};
            }
        }

        protected override bool OnBackButtonPressed()
        {
            ViewModel.CancelCommand.Execute(null);

            return true;
        }

        public NewManualActivityViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                BindingContext = ViewModel;
            }
        }
    }
}