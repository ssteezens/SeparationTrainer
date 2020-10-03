using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SeparationTrainer.Data.Entities
{
    public class Activity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int AnxietyLevel { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public string Notes { get; set; }

        public DateTime Created { get; set; }
    }
}
