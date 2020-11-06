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
            GoToEditActivityCommand = new Command<ActivityModel>(async (activity) => await GoToEditActivity(activity));
            DeleteActivityCommand = new Command<ActivityModel>(async (activity) => await DeleteActivity(activity));
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
            var currentYear = DateTime.Now.Year;
            var yearStart = new DateTime(currentYear, 1, 1);
            var today = DateTime.Now.Date;

            for (var currentDay = yearStart; currentDay.Date <= today.Date; currentDay = currentDay.AddDays(1))
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
                    activitiesForDay = activityGroups?.SingleOrDefault(group => group.Key == session.Created.Date)?.ToList();

                session.Activities = activitiesForDay;
            }
        }

        public async Task GetNewActivitiesForSession()
        {
            var activitiesForDay = await ActivityService.GetForDayAsync(CurrentSession.Created.Date);
            var newActivities = activitiesForDay.Where(activity => !CurrentSession.Activities.Select(i => i.Id).Contains(activity.Id));

            CurrentSession.Activities.AddRange(newActivities);
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

        public Command<ActivityModel> GoToEditActivityCommand { get; }

        public Command<ActivityModel> DeleteActivityCommand { get; }

        public async Task GoToAddActivity()
        {
            await Shell.Current.GoToAsync($"//{nameof(NewActivityPage)}");
        }

        public async Task GoToEditActivity(ActivityModel activity)
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            await Shell.Current.GoToAsync($"//{nameof(EditActivityPage)}?activityId={activity.Id}");
        }

        public async Task DeleteActivity(ActivityModel activity)
        {

        }
    }
}
