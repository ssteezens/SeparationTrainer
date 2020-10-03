using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace SeparationTrainer.Data.Entities
{
    public class Session
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public List<Activity> Activities { get; set; }
    }
}
