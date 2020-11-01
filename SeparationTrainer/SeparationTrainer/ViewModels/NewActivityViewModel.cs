using SeparationTrainer.Data.Entities;
using SeparationTrainer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
using SeparationTrainer.Models;
using SeparationTrainer.Shared;
using Xamarin.Forms;
using ActivityModel = SeparationTrainer.Models.ActivityModel;
using Tag = SeparationTrainer.Data.Entities.Tag;

namespace SeparationTrainer.ViewModels
{
    public class NewActivityViewModel : BaseViewModel
    {
        private bool _stopWatchIsRunning;
        private string _notes;
        private int _selectedStressLevel = 1;
        private TimeSpan _elapsedTime = TimeSpan.MinValue;
        private ObservableCollection<TagModel> _availableTags = new ObservableCollection<TagModel>();
        private ObservableCollection<ActivityTag> _appliedTags = new ObservableCollection<ActivityTag>();

        public NewActivityViewModel()
        {
            StartStopStopWatchCommand = new Command(StartStopStopWatch);
            ResetTimerCommand = new Command(ResetTimer);
            CancelCommand = new Command(async() => await Cancel());
            SaveActivityCommand = new Command(async () => await SaveActivity(), () => CanSaveActivity);
            AddNewTagCommand = new Command(async () => await  AddNewTag());
            ShowMoreTagsCommand = new Command(async () => await ShowMoreTags());

            StopWatchTimer = new Timer(100) { Enabled = false }; 
            StopWatchTimer.Elapsed +=  OnStopWatchTimerOnElapsed;
        }

        public void ResetPage()
        {
            ResetTimer();
            SelectedStressLevel = 1;
            Notes = string.Empty;
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

        public ObservableCollection<ActivityTag> AppliedTags
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

            OnPropertyChanged(nameof(ResetTimerIsEnabled));
            OnPropertyChanged(nameof(StartStopButtonText));
            SaveActivityCommand.ChangeCanExecute();
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
                ElapsedTime = ElapsedTime
            };
            var sharedModel = Mapper.Map<Shared.ActivityModel>(activity);

            await ActivityService.AddAsync(sharedModel);

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

            var tag = new Models.Tag { Name = result };

            // todo: call tag service to create tag or get tag id

            var activityTag = new ActivityTag()
            {
                Tag = tag,
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
        }

        #endregion
    }
}
