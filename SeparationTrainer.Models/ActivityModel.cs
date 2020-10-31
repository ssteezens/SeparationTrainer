using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparationTrainer.Models
{
    public class ActivityModel
    {
        public int Id { get; set; }

        public int AnxietyLevel { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public string Notes { get; set; }

        public DateTime Created { get; set; }

        public List<ActivityTagModel> Tags { get; set; }
    }
}
