using SeparationTrainer.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SeparationTrainer.Models
{
    public class SessionModel : ObservableObject
    {
        private ObservableCollection<ActivityModel> _activities = new ObservableCollection<ActivityModel>();

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public ObservableCollection<ActivityModel> Activities
        {
            get => _activities;
            set
            {
                SetProperty(ref _activities, value, nameof(Activities));

                OnPropertyChanged(nameof(TotalActivityTime));
                OnPropertyChanged(nameof(TotalTimeDisplay));
                OnPropertyChanged(nameof(HasActivities));
            }
        }

        public bool HasActivities => Activities.Count > 0;

        public string TotalTimeDisplay => TotalActivityTime.ToShortForm();

        public TimeSpan TotalActivityTime => new TimeSpan(Activities.Where(i => i.ElapsedTime > TimeSpan.MinValue).Sum(i => i.ElapsedTime.Ticks));
    }
}
