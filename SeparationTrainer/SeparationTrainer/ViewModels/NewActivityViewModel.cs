using SeparationTrainer.Extensions;
using SeparationTrainer.Models;
using SeparationTrainer.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using ActivityModel = SeparationTrainer.Models.ActivityModel;
using ActivityTagModel = SeparationTrainer.Models.ActivityTagModel;

namespace SeparationTrainer.ViewModels
{
    public class NewActivityViewModel : BaseViewModel
    {
        private bool _stopWatchIsRunning;
        private int _lastNotificationSecond = 0;
        private string _notes;
        private int _selectedStressLevel = 1;
        private TimeSpan _elapsedTime = TimeSpan.MinValue;
        private ObservableCollection<TagModel> _availableTags = new ObservableCollection<TagModel>();
        private ObservableCollection<ActivityTagModel> _appliedTags = new ObservableCollection<ActivityTagModel>();

        public NewActivityViewModel()
        {
            StartStopStopWatchCommand = new Command(StartStopStopWatch);
            ResetTimerCommand = new Command(ResetTimer);
            CancelCommand = new Command(async() => await Cancel());
            SaveActivityCommand = new Command(async () => await SaveActivity(), () => CanSaveActivity);
            AddNewTagCommand = new Command(async () => await  AddNewTag());
            RemoveTagCommand = new Command<TagModel>(RemoveTag);
            ShowMoreTagsCommand = new Command(async () => await ShowMoreTags());

            StopWatchTimer = new Timer(100) { Enabled = false }; 
            StopWatchTimer.Elapsed +=  OnStopWatchTimerOnElapsed;
        }

        public void ResetPage()
        {
            ResetTimer();
            SelectedStressLevel = 1;
            Notes = string.Empty;
            NotificationManager.ClearNotification(0);
        }

        #region Properties

        public string StartStopButtonText => StopWatchIsRunning ? "Stop" : "Start";

        public bool StopWatchIsRunning
        {
            get => _stopWatchIsRunning;
            set => SetProperty(ref _stopWatchIsRunning, value, nameof(StopWatchIsRunning));
        }

        public bool CanSaveActivity => !StopWatchIsRunning && ElapsedTime > TimeSpan.MinValue;

        public TimeSpan ElapsedTime
        {
            get => _elapsedTime;
            set => SetProperty(ref _elapsedTime, value, nameof(ElapsedTime));
        }

        public int SelectedStressLevel
        {
            get => _selectedStressLevel;
            set => SetProperty(ref _selectedStressLevel, value, nameof(SelectedStressLevel));
        }

        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value, nameof(Notes));
        }

        public bool ResetTimerIsEnabled => StopWatchIsRunning || ElapsedTime != TimeSpan.MinValue;

        public DateTime TimerStart { get; set; } = DateTime.MinValue;

        public Timer StopWatchTimer { get; set; }

        public ObservableCollection<TagModel> AvailableTags
        {
            get => _availableTags;
            set => SetProperty(ref _availableTags, value, nameof(AvailableTags));
        }

        public ObservableCollection<ActivityTagModel> AppliedTags
        {
            get => _appliedTags;
            set => SetProperty(ref _appliedTags, value, nameof(AppliedTags));
        }

        #endregion

        #region Commands

        public Command StartStopStopWatchCommand { get; }

        public Command ResetTimerCommand { get; }

        public Command CancelCommand { get; }

        public Command SaveActivityCommand { get; }

        public Command AddNewTagCommand { get; }

        public Command ShowMoreTagsCommand { get; }

        public Command<TagModel> RemoveTagCommand { get; }

        private void RemoveTag(TagModel tagToRemove)
        {
            var activityTagToRemove = AppliedTags.SingleOrDefault(tag => tag.TagModel == tagToRemove);

            if (activityTagToRemove != null)
            {
                AppliedTags.Remove(activityTagToRemove);
            }
        }

        private void StartStopStopWatch()
        {
            // set the timer start if not set
            if (TimerStart == DateTime.MinValue)
                TimerStart = DateTime.Now;

            StopWatchIsRunning = !StopWatchIsRunning;
            StopWatchTimer.Enabled = StopWatchIsRunning;

            OnPropertyChanged(nameof(ResetTimerIsEnabled));
            OnPropertyChanged(nameof(StartStopButtonText));
            SaveActivityCommand.ChangeCanExecute();
        }

        private void ResetTimer()
        {
            StopWatchTimer.Elapsed -= OnStopWatchTimerOnElapsed;
            StopWatchTimer = new Timer(100) { Enabled = false };
            StopWatchTimer.Elapsed += OnStopWatchTimerOnElapsed;

            StopWatchIsRunning = false;
            StopWatchTimer.Enabled = false;
            ElapsedTime = TimeSpan.MinValue;
            TimerStart = DateTime.MinValue;
            AppliedTags = new ObservableCollection<ActivityTagModel>();

            OnPropertyChanged(nameof(ResetTimerIsEnabled));
            OnPropertyChanged(nameof(StartStopButtonText));
            SaveActivityCommand.ChangeCanExecute();
            _lastNotificationSecond = 0;
        }

        private async Task Cancel()
        {
            var result = await DialogService.ShowAlert("Cancel?", "Are you sure you would like to cancel this activity?");

            if (!result)
                return;

            await Shell.Current.GoToAsync($"//{nameof(ViewSessionsPage)}");

            ResetPage();
        }

        private async Task SaveActivity()
        {
            var activity = new ActivityModel()
            {
                AnxietyLevel = SelectedStressLevel,
                Created = DateTime.Now,
                Notes = Notes,
                ElapsedTime = ElapsedTime,
                Tags = new ObservableCollection<ActivityTagModel>(AppliedTags.ToList())
            };
            
            await ActivityService.AddAsync(activity);

            ResetPage();

            await Shell.Current.GoToAsync($"//{nameof(ViewSessionsPage)}");
        }

        private async Task AddNewTag()
        {
            var result = await DialogService.DisplayPrompt("Add Tag", 
                "Enter a new tag for this activity", 
                "Ok",
                "Cancel", 
                "New Tag Here", 
                100);

            if (result == null)
                return;

            var tagModel = new TagModel { Name = result };
            var returnedTagModel = await TagService.AddAsync(tagModel);

            tagModel.Id = returnedTagModel.Id;

            var activityTag = new ActivityTagModel()
            {
                TagModel = tagModel,
                AppliedOn = DateTime.Now
            };

            AppliedTags.Add(activityTag);
        }

        private async Task ShowMoreTags()
        {

        }

        private void OnStopWatchTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            ElapsedTime = e.SignalTime - TimerStart;

            // update notification every second
            if ((int)ElapsedTime.TotalSeconds > _lastNotificationSecond)
            {
                var timerText = ElapsedTime.ToShortStopwatchForm();

                NotificationManager.SendNotification("Activity Started", timerText, messageId: 0);
                _lastNotificationSecond = (int)ElapsedTime.TotalSeconds;
            }
        }

        #endregion
    }
}
