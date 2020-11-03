using System;
using System.Collections.Generic;

namespace SeparationTrainer.Models
{
    public class ActivityModel : ObservableObject
    {
        private int _anxietyLevel;
        private TimeSpan _elapsedTime = new TimeSpan(0, 0, 0, 1);
        private string _notes = string.Empty;
        private DateTime _created;
        private List<ActivityTagModel> _tags;
        public int Id { get; set; }

        public int AnxietyLevel
        {
            get => _anxietyLevel;
            set => SetProperty(ref _anxietyLevel, value, nameof(AnxietyLevel));
        }

        public TimeSpan ElapsedTime
        {
            get => _elapsedTime;
            set => SetProperty(ref _elapsedTime, value, nameof(ElapsedTime));
        }

        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value, nameof(Notes));
        }

        public DateTime Created
        {
            get => _created;
            set => SetProperty(ref _created, value, nameof(Created));
        }

        public List<ActivityTagModel> Tags
        {
            get => _tags;
            set => SetProperty(ref _tags, value, nameof(Tags));
        }

        public bool HasTags => Tags != null && Tags.Count > 0;
    }
}
