using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Extensions;

namespace SeparationTrainer.Models
{
    public class SessionModel : ObservableObject
    {
        private List<ActivityModel> _activities = new List<ActivityModel>();

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public List<ActivityModel> Activities
        {
            get => _activities;
            set
            {
                SetProperty(ref _activities, value, nameof(Activities));

                OnPropertyChanged(nameof(TotalActivityTime));
                OnPropertyChanged(nameof(TotalTimeDisplay));
            }
        }

        public string TotalTimeDisplay => TotalActivityTime.ToShortForm();

        public TimeSpan TotalActivityTime => new TimeSpan(Activities.Where(i => i.ElapsedTime > TimeSpan.MinValue).Sum(i => i.ElapsedTime.Ticks));
    }
}
