using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using SeparationTrainer.Models;
using Xamarin.Forms;

namespace SeparationTrainer.ViewModels
{
    public class NewActivityViewModel : BaseViewModel
    {
        private string _timerText;
        private bool _stopWatchIsRunning;

        public NewActivityViewModel()
        {
            StartStopStopWatchCommand = new Command(StartStopStopWatch);
            SaveActivityCommand = new Command(SaveActivity);

            StopWatchTimer = new Timer(100) { Enabled = false }; 
            StopWatchTimer.Elapsed +=  OnStopWatchTimerOnElapsed;
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

        public string SelectedStressLevel { get; set; }

        public string Notes { get; set; }

        public Timer StopWatchTimer { get; set; }

        public Command StartStopStopWatchCommand { get; }

        public Command SaveActivityCommand { get; }

        public DateTime TimerStart { get; set; } = DateTime.MinValue;

        private void StartStopStopWatch()
        {
            // set the timer start if not set
            if (TimerStart == DateTime.MinValue)
                TimerStart = DateTime.Now;

            StopWatchIsRunning = !StopWatchIsRunning;
            StopWatchTimer.Enabled = StopWatchIsRunning;

            OnPropertyChanged(nameof(StartStopButtonText));
        }

        private void SaveActivity()
        {
            var activity = new ActivityModel()
            {
                AnxietyLevel = int.Parse(SelectedStressLevel),
                Created = DateTime.Now,
                Notes = Notes,
                ElapsedTime = ElapsedTime
            };

            // todo: save activity in database
        }

        private void OnStopWatchTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            ElapsedTime = e.SignalTime - TimerStart;
            TimerText = ElapsedTime.ToString("g");
        }
    }
}
