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
        private string _timerText = "0:00:00.00";
        private bool _stopWatchIsRunning;
        private string _notes;
        private string _selectedStressLevel = "1";

        public NewActivityViewModel()
        {
            StartStopStopWatchCommand = new Command(StartStopStopWatch);
            SaveActivityCommand = new Command(async () => await SaveActivity());

            StopWatchTimer = new Timer(100) { Enabled = false }; 
            StopWatchTimer.Elapsed +=  OnStopWatchTimerOnElapsed;
        }

        public void Refresh()
        {
            TimerText = "0:00:00.00";
            TimerStart = DateTime.MinValue;
            Notes = string.Empty;
            SelectedStressLevel = "1";
            OnPropertyChanged(nameof(StartStopButtonText));
        }

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

        public TimeSpan ElapsedTime { get; set; }

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

        public DateTime TimerStart { get; set; } = DateTime.MinValue;

        public Timer StopWatchTimer { get; set; }

        public Command StartStopStopWatchCommand { get; }

        public Command SaveActivityCommand { get; }

        private void StartStopStopWatch()
        {
            // set the timer start if not set
            if (TimerStart == DateTime.MinValue)
                TimerStart = DateTime.Now;

            StopWatchIsRunning = !StopWatchIsRunning;
            StopWatchTimer.Enabled = StopWatchIsRunning;

            OnPropertyChanged(nameof(StartStopButtonText));
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
            await Shell.Current.GoToAsync($"//{nameof(ActivitiesPage)}");
        }

        private void OnStopWatchTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            ElapsedTime = e.SignalTime - TimerStart;
            TimerText = ElapsedTime.ToString(@"hh\:mm\:ss\.ff");
        }
    }
}
