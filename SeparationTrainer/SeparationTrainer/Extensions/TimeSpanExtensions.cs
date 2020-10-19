using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparationTrainer.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToShortForm(this TimeSpan t)
        {
            string shortForm = "";

            if (t.Hours > 0)
            {
                shortForm += $"{t.Hours.ToString()}h ";
            }
            if (t.Minutes > 0)
            {
                shortForm += $"{t.Minutes.ToString()}m ";
            }
            
            shortForm += $"{t.Seconds.ToString()}s";

            return shortForm;
        }
    }
}
