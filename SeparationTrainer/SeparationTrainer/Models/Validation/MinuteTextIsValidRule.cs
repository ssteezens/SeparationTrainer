namespace SeparationTrainer.Models.Validation
{
    public class MinuteTextIsValidRule<T> : IValidationRule<T>
    {
        public MinuteTextIsValidRule(string validationMessage = "")
        {
            ValidationMessage = validationMessage;
        }

        public string ValidationMessage { get; set; }

        public bool Check<T>(T value)
        {
            if (value == null)
                return false;

            var stringValue = value as string;
            var valueIsInt = int.TryParse(stringValue, out var intValue);

            if (!valueIsInt)
                return false;

            return intValue >= 0 && intValue <= 59;
        }
    }
}