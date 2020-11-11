using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparationTrainer.Models.Validation
{
    public class HourTextIsValidRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check<T>(T value)
        {
            if (value == null)
                return false;

            var stringValue = value as string;
            var valueIsInt = int.TryParse(stringValue, out var intValue);

            if (!valueIsInt)
                return false;

            return intValue >= 0 && intValue <= 24;
        }
    }
}
