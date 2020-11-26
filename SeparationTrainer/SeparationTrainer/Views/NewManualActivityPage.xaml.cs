using SeparationTrainer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeparationTrainer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewManualActivityPage : ContentPage
    {
        public NewManualActivityPage()
        {
            InitializeComponent();
            ViewModel = new NewManualActivityViewModel();

            this.BindingContext = ViewModel;
        }

        public NewManualActivityViewModel ViewModel { get; set; }
    }
}