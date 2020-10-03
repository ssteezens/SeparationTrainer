﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparationTrainer.Data.Entities
{
    public class Session
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public List<Activity> Activities { get; set; }
    }
}
