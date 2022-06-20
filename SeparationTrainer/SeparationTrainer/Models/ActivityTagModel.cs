using System;
using Xamarin.Forms;

namespace SeparationTrainer.Models
{
    public class ActivityTagModel : ObservableObject
    {
        private TagModel _tagModel;
        private DateTime _appliedOn;

        public int TagId { get; set; }

        public TagModel TagModel
        {
            get => _tagModel;
            set => SetProperty(ref _tagModel, value, nameof(TagModel));
        }

        public DateTime AppliedOn
        {
            get => _appliedOn;
            set => SetProperty(ref _appliedOn, value, nameof(AppliedOn));
        }

        public Command RemoveTagCommand { get; set; }
    }
}
