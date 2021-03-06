﻿using SQLite;
using System;
using System.Collections.Generic;

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
