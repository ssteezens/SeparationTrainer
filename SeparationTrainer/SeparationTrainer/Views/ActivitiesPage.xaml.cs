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
    public partial class ActivitiesPage : ContentPage
    {
        public ActivitiesPage()
        {
            InitializeComponent();

            ViewModel = new ViewActivitiesViewModel();
            this.BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            ViewModel?.GetActivities();

            // scroll to the last activity
            if (ViewModel?.Activities.Count > 0)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ActivityCollectionView.ScrollTo(ViewModel.Activities.Last(), ScrollToPosition.End);
                });
            }
        }

        public ViewActivitiesViewModel ViewModel { get; set; }
    }
}