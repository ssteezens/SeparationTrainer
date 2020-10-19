using SeparationTrainer.Data.Repositories;
using SeparationTrainer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SeparationTrainer.Views;
using Xamarin.Forms;

namespace SeparationTrainer.ViewModels
{
    public class ViewActivitiesViewModel : BaseViewModel
    {
        private ObservableCollection<ActivityModel> _activities;
        private bool _isRefreshing;
        private ObservableCollection<SessionModel> _sessions;

        public ViewActivitiesViewModel()
        {
            RefreshCommand = new Command(GetActivities);
            GoToAddActivityCommand = new Command(async () => await GoToAddActivity());
        }

        public override async Task LoadData()
        {
            IsBusy = true;
            IsRefreshing = true;

            var activities = await ActivityRepository.GetAllAsync();
            var activityModels = Mapper.Map<List<ActivityModel>>(activities);

            var sessions = new List<SessionModel>();

            var activityGroups = activityModels.GroupBy(i => i.Created.Date);

            foreach (var group in activityGroups)
            {
                var session = new SessionModel()
                {
                    Created = group.Key,
                    Activities = group.Select(i => i).ToList(),
                    Title = group.Key.ToString("D"),
                    Description = $"Activities for {@group.Key:D}"
                };

                sessions.Add(session);
            }

            Activities = new ObservableCollection<ActivityModel>(activityModels);
            Sessions = new ObservableCollection<SessionModel>(sessions);

            IsRefreshing = false;
            IsBusy = false;
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

        public ObservableCollection<SessionModel> Sessions
        {
            get => _sessions;
            set => SetProperty(ref _sessions, value, nameof(Sessions));
        }

        public Command RefreshCommand { get; }

        public Command GoToAddActivityCommand { get; }

        public void GetActivities()
        {
            IsRefreshing = true;

            var activities = ActivityRepository.GetAll();
            var activityModels = Mapper.Map<List<ActivityModel>>(activities);

            Activities = new ObservableCollection<ActivityModel>(activityModels);

            IsRefreshing = false;
        }

        public async Task GoToAddActivity()
        {
            await Shell.Current.GoToAsync($"//{nameof(NewActivityPage)}");
        }
    }
}
