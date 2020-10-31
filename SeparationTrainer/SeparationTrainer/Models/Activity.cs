using System;
using System.Collections.Generic;

namespace SeparationTrainer.Models
{
    public class ActivityModel
    {
        public int Id { get; set; }
        
        public int AnxietyLevel { get; set; }

        public TimeSpan ElapsedTime { get; set; } = new TimeSpan(0, 0, 0, 1);

        public string Notes { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
