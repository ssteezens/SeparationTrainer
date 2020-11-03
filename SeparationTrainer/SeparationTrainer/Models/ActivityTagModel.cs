using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeparationTrainer.Models
{
    public class ActivityTagModel : ObservableObject
    {
        private TagModel _tagModel;
        private DateTime _appliedOn;

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
    }
}
