using SeparationTrainer.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeparationTrainer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserSettingsPage : ContentPage
    {
        private UserSettingsViewModel _userSettings;

        public UserSettingsPage()
        {
            InitializeComponent();

            ViewModel = new UserSettingsViewModel();
        }

        public UserSettingsViewModel ViewModel
        {
            get => _userSettings;
            set
            {
                _userSettings = value;
                BindingContext = ViewModel;
            }
        }
    }
}