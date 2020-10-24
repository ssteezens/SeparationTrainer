using SeparationTrainer.Data.Entities;
using SeparationTrainer.Models;
using SeparationTrainer.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;

namespace SeparationTrainer.ViewModels
{
    public class NewActivityViewModel : BaseViewModel
    {
        private string _timerText = "00:00.00";
        private bool _stopWatchIsRunning;
        private string _notes;
        private string _selectedStressLevel = "1";

        public NewActivityViewModel()
        {
            StartStopStopWatchCommand = new Command(StartStopStopWatch);
            ResetTimerCommand = new Command(ResetTimer);
            CancelCommand = new Command(async() => await Cancel());
            SaveActivityCommand = new Command(async () => await SaveActivity(), () => CanSaveActivity);

            StopWatchTimer = new Timer(100) { Enabled = false }; 
            StopWatchTimer.Elapsed +=  OnStopWatchTimerOnElapsed;
        }

        public void ResetPage()
        {
            ResetTimer();
            SelectedStressLevel = "1";
            Notes = string.Empty;
        }

        #region Properties

        public string TimerText
        {
            get => _timerText;
            set => SetProperty(ref _timerText, value, nameof(TimerText));
        }

        public string StartStopButtonText => StopWatchIsRunning ? "Stop" : "Start";

        public bool StopWatchIsRunning
        {
            get => _stopWatchIsRunning;
            set => SetProperty(ref _stopWatchIsRunning, value, nameof(StopWatchIsRunning));
        }

        public bool CanSaveActivity => !StopWatchIsRunning && !string.IsNullOrEmpty(SelectedStressLevel) && ElapsedTime > TimeSpan.MinValue;

        public TimeSpan ElapsedTime { get; set; } = TimeSpan.MinValue;

        public List<string> StressLevels => new List<string>() { "1", "2", "3", "4", "5", "6", "7" };

        public string SelectedStressLevel
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

        #endregion

        #region Commands

        public Command StartStopStopWatchCommand { get; }

        public Command ResetTimerCommand { get; }

        public Command CancelCommand { get; }

        public Command SaveActivityCommand { get; }

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
            TimerText = "00:00.00";

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
                AnxietyLevel = int.Parse(SelectedStressLevel),
                Created = DateTime.Now,
                Notes = Notes,
                ElapsedTime = ElapsedTime
            };

            var entity = Mapper.Map<Activity>(activity);

            await ActivityRepository.Add(entity);

            ResetPage();

            await Shell.Current.GoToAsync($"//{nameof(ViewSessionsPage)}");
        }

        private void OnStopWatchTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            ElapsedTime = e.SignalTime - TimerStart;
            TimerText = ElapsedTime.ToString(@"hh\:mm\:ss\.ff");
        }

        #endregion
    }
}
