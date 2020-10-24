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

        public TimeSpan ElapsedTime { get; set; } = new TimeSpan(0, 0, 0, 1);

        public string Notes { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public bool UsedExitDoor { get; set; }

        public bool InSightSeparation { get; set; }
    }
}
