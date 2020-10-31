using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparationTrainer.Data.Entities
{
    public class ActivityTags
    {
        public int TagId { get; set; }

        public int ActivityId { get; set; }

        public DateTime AppliedOn { get; set; }
    }
}
