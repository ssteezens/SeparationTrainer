using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Models;
using Xamarin.Forms;

namespace SeparationTrainer.ViewModels
{
    public class EditActivityViewModel : BaseViewModel
    {
        private ActivityModel _activityToEdit = new ActivityModel();
        private string _selectedStressLevel = "1";

        public EditActivityViewModel()
        {
            UpdateActivityCommand = new Command(async () => await UpdateActivity());
        }

        public List<string> StressLevels => new List<string>() { "1", "2", "3", "4", "5", "6", "7" };

        public string SelectedStressLevel
        {
            get => _selectedStressLevel;
            set => SetProperty(ref _selectedStressLevel, value, nameof(SelectedStressLevel));
        }

        public ActivityModel ActivityToEdit
        {
            get => _activityToEdit;
            set => SetProperty(ref _activityToEdit, value, nameof(ActivityToEdit));
        }

        public Command UpdateActivityCommand { get; }

        private async Task UpdateActivity()
        {
            var entity = await ActivityRepository.GetAsync(ActivityToEdit.Id);
            if (entity == null)
                return;

            entity.AnxietyLevel = int.Parse(SelectedStressLevel);
            entity.Notes = ActivityToEdit.Notes;

            await ActivityRepository.UpdateAsync(entity);
        }
    }
}
