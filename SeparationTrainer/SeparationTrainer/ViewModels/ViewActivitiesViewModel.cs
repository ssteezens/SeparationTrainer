using SeparationTrainer.Data.Repositories;
using SeparationTrainer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace SeparationTrainer.ViewModels
{
    public class ViewActivitiesViewModel : BaseViewModel
    {
        private ObservableCollection<ActivityModel> _activities;
        private bool _isRefreshing;

        public ViewActivitiesViewModel()
        {
            RefreshCommand = new Command(GetActivities);

            GetActivities();
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value, nameof(IsRefreshing));
        }

        public ObservableCollection<ActivityModel> Activities
        {
            get => _activities;
            set => SetProperty(ref _activities, value, nameof(Activities));
        }

        public Command RefreshCommand { get; }

        public void GetActivities()
        {
            IsRefreshing = true;

            var activities = ActivityRepository.GetAll();
            var activityModels = Mapper.Map<List<ActivityModel>>(activities);

            Activities = new ObservableCollection<ActivityModel>(activityModels);

            IsRefreshing = false;
        }
    }
}
