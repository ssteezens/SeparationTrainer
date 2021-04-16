using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparationTrainer.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToShortForm(this TimeSpan time)
        {
            string shortForm = "";

            if (time.Hours > 0)
            {
                shortForm += $"{time.Hours.ToString()}h ";
            }
            if (time.Minutes > 0)
            {
                shortForm += $"{time.Minutes.ToString()}m ";
            }
            
            shortForm += $"{time.Seconds.ToString()}s";

            return shortForm;
        }

        public static string ToStopwatchForm(this TimeSpan time)
        {
            var hoursText = time.Hours > 0 ?
                $"{time.Hours.ToString().PadLeft(2, '0')}:"
                : string.Empty;
            var minutesText = time.Minutes > 0 ?
                $"{time.Minutes.ToString().PadLeft(2, '0')}:"
                : string.Empty;
            var secondsText = time.Seconds > 0 ?
                $"{time.Seconds.ToString().PadLeft(2, '0')}:"
                : "00:";
            var hundredths = (int)Math.Round((double)time.Milliseconds / 10, 2);
            var millisecondsText = (time.Milliseconds > 0 && time.Hours == 0) ?
                hundredths.ToString().PadLeft(2, '0')
                : "00";

            return $"{hoursText}{minutesText}{secondsText}{millisecondsText}";
        }

        public static string ToShortStopwatchForm(this TimeSpan time)
        {
            var hoursText = time.Hours > 0 ?
                $"{time.Hours.ToString().PadLeft(2, '0')}:"
                : string.Empty;
            var minutesText = time.Minutes > 0 ?
                $"{time.Minutes.ToString().PadLeft(2, '0')}:"
                : "00:";
            var secondsText = time.Seconds > 0 ?
                $"{time.Seconds.ToString().PadLeft(2, '0')}"
                : "00";

            return $"{hoursText}{minutesText}{secondsText}";
        }
    }
}
