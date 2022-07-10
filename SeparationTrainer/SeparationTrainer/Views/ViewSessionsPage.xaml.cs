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

        ViewCell lastCell;
        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            //if (lastCell != null)
            //    lastCell.View.BackgroundColor = Color.Transparent;
            //var viewCell = (ViewCell)sender;
            //if (viewCell.View != null)
            //{
            //    switch (AppState.CurrentTheme)
            //    {
            //        case Theme.Dark:
            //            viewCell.View.BackgroundColor = Color.FromHex("#322c3a");
            //            break;
            //        case Theme.Light:
            //        default:
            //            viewCell.View.BackgroundColor = Color.LightGray;
            //            break;
            //    }
                
            //    lastCell = viewCell;
            //}
        }
    }
}