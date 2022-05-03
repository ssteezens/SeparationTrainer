using SeparationTrainer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeparationTrainer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("ActivityId", "activityId")]
    public partial class EditActivityPage : ContentPage
    {
        private string _activityId;
        private EditActivityViewModel _viewModel;

        public EditActivityPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            ViewModel.CancelCommand.Execute(null);

            return true;
        }

        protected override async void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            await ViewModel.LoadData();
        }

        public string ActivityId
        {
            get => _activityId;
            set
            {
                _activityId = value;
                ViewModel = new EditActivityViewModel { ActivityToEditId =  int.Parse(ActivityId) };
            }
        }

        public EditActivityViewModel ViewModel
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