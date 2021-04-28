using SeparationTrainer.Models;
using SeparationTrainer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeparationTrainer.ViewModels
{
    public class ViewSessionsViewModel : BaseViewModel
    {
        private ObservableCollection<SessionModel> _sessions = new ObservableCollection<SessionModel>();
        private SessionModel _currentSession;

        public ViewSessionsViewModel()
        {
            GoToAddActivityCommand = new Command(async () => await GoToAddActivity());
            GoToAddManualActivityCommand = new Command(async () => await GoToAddManualActivity());
            GoToEditActivityCommand = new Command<ActivityModel>(async (activity) => await GoToEditActivity(activity));
            DeleteActivityCommand = new Command<ActivityModel>(async (activity) => await DeleteActivity(activity));
            GoToTodayCommand = new Command(GoToToday);

            MessagingCenter.Subscribe<EditActivityViewModel, ActivityModel>(this, "EditActivity", OnEditActivityMessageReceived);
        }

        private void OnEditActivityMessageReceived(EditActivityViewModel sender, ActivityModel model)
        {
            foreach (var session in Sessions)
            {
                var activityToEdit = session.Activities.FirstOrDefault(i => i.Id == model.Id);

                if (activityToEdit != null)
                {
                    activityToEdit.AnxietyLevel = model.AnxietyLevel;
                    activityToEdit.ElapsedTime = model.ElapsedTime;
                    activityToEdit.Tags = model.Tags;
                    break;
                }
            }

        }

        public async Task OnAppearing()
        {
            IsBusy = true;

            if(!Sessions.Any())
            {
                await OnInitialLoad();
            }
            else
            {
                await GetNewActivitiesForSession();
            }

            IsBusy = false;
        }

        public async Task OnInitialLoad()
        {
            LoadSessions();
            CurrentSession = Sessions.Single(i => i.Created.Date == DateTime.Now.Date);

            await LoadSessionActivities();
        }

        public void LoadSessions()
        {
            var startDate = DateTime.Today.AddDays(-365);
            var today = DateTime.Now.Date;

            for (var currentDay = startDate; currentDay.Date <= today.Date; currentDay = currentDay.AddDays(1))
            {
                var session = new SessionModel()
                {
                    Created = currentDay,
                    Title = currentDay.ToString("D"),
                    Description = $"Activities for {currentDay:D}"
                };

                Sessions.Add(session);
            }
        }

        public async Task LoadSessionActivities()
        {
            var activityModels = await ActivityService.GetAllAsync();
            var activityGroups = activityModels.GroupBy(i => i.Created.Date).ToList();

            if (!Sessions.Any())
                return;

            foreach (var session in Sessions)
            {
                var activitiesForDay = new List<ActivityModel>();

                if (activityGroups.SingleOrDefault(group => group.Key == session.Created.Date) != null)
                    activitiesForDay = activityGroups?.SingleOrDefault(group => group.Key == session.Created.Date)?.ToList() ?? new List<ActivityModel>();

                session.Activities = new ObservableCollection<ActivityModel>(activitiesForDay);
            }
        }

        public async Task GetNewActivitiesForSession()
        {
            var activitiesForDay = await ActivityService.GetForDayAsync(CurrentSession.Created.Date);
            var newActivities = activitiesForDay.Where(activity => !CurrentSession.Activities.Select(i => i.Id).Contains(activity.Id));

            foreach(var activity in newActivities)
                CurrentSession.Activities.Add(activity);

            CurrentSession.OnPropertyChanged(nameof(CurrentSession.HasActivities));
        }

        public ObservableCollection<SessionModel> Sessions
        {
            get => _sessions;
            set => SetProperty(ref _sessions, value, nameof(Sessions));
        }

        public SessionModel CurrentSession
        {
            get => _currentSession;
            set => SetProperty(ref _currentSession, value, nameof(CurrentSession));
        }

        public Command GoToAddActivityCommand { get; }

        public Command GoToAddManualActivityCommand { get; }

        public Command<ActivityModel> GoToEditActivityCommand { get; }

        public Command<ActivityModel> DeleteActivityCommand { get; }

        public Command GoToTodayCommand { get; }

        public void GoToToday()
        {
            CurrentSession = Sessions.Single(i => i.Created.Date == DateTime.Today);
        }

        public async Task GoToAddActivity()
        {
            await Shell.Current.GoToAsync($"//{nameof(NewActivityPage)}");
        }

        public async Task GoToAddManualActivity()
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            await Shell.Current.GoToAsync($"//{nameof(NewManualActivityPage)}?selectedDate={CurrentSession.Created:d}");
        }

        public async Task GoToEditActivity(ActivityModel activity)
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            await Shell.Current.GoToAsync($"//{nameof(EditActivityPage)}?activityId={activity.Id}");
        }

        public async Task DeleteActivity(ActivityModel activity)
        {
            var result = await DialogService.ShowAlert("Cancel?", "Are you sure you would like to cancel this activity?");
            if (!result)
                return;

            await ActivityService.DeleteAsync(activity.Id);

            CurrentSession.Activities.Remove(activity);
            CurrentSession.OnPropertyChanged(nameof(CurrentSession.HasActivities));
        }
    }
}
