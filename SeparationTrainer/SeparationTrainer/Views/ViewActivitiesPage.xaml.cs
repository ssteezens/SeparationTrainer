﻿using System;
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
    public partial class ViewActivitiesPage : ContentPage
    {
        public ViewActivitiesPage()
        {
            InitializeComponent();

            ViewModel = new ViewActivitiesViewModel();
            this.BindingContext = ViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel != null)
            {
                await ViewModel?.LoadData();

                // scroll to the last activity
                if (ViewModel?.Activities.Count > 0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ActivityCollectionView.ScrollTo(ViewModel.Activities.Last(), ScrollToPosition.End);
                    });
                }
            }
        }

        public ViewActivitiesViewModel ViewModel { get; set; }
    }
}