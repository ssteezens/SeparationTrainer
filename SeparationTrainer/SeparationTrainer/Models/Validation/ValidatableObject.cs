using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparationTrainer.Models.Validation
{
    public class ValidatableObject<T> : ObservableObject
    {
        private List<IValidationRule<T>> _validations = new List<IValidationRule<T>>();
        private T _value;
        private bool _isValid = true;
        private ObservableCollection<string> _errors = new ObservableCollection<string>();

        public List<IValidationRule<T>> Validations
        {
            get => _validations;
            set => _validations = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value, nameof(Value));
                Validate();
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value, nameof(IsValid));
        }

        public ObservableCollection<string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value, nameof(Errors));
        }

        public bool Validate()
        {
            Errors.Clear();

            Errors = new ObservableCollection<string>(Validations.Where(i => !i.Check(Value)).Select(i => i.ValidationMessage).ToList());
            IsValid = !Errors.Any();

            return IsValid;
        }
    }
}
