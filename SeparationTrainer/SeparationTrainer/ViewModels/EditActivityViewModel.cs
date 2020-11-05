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
        private string _hoursText;
        private string _minutesText;
        private string _secondsText;

        public EditActivityViewModel()
        {
            UpdateActivityCommand = new Command(async () => await UpdateActivity());
        }

        public override async Task LoadData()
        {
            if (ActivityToEditId > 0)
            {
                var activity = await ActivityService.GetAsync(ActivityToEditId);

                ActivityToEdit = activity;

                HoursText = ActivityToEdit.ElapsedTime.Hours.ToString().PadLeft(2, '0');
                MinutesText = ActivityToEdit.ElapsedTime.Minutes.ToString().PadLeft(2, '0');
                SecondsText = ActivityToEdit.ElapsedTime.Seconds.ToString().PadLeft(2, '0');
            }
        }

        public int ActivityToEditId { get; set; }

        public List<string> StressLevels => new List<string>() { "1", "2", "3", "4", "5", "6", "7" };

        public string HoursText
        {
            get => _hoursText;
            set => SetProperty(ref _hoursText, value, nameof(HoursText));
        }

        public string MinutesText
        {
            get => _minutesText;
            set => SetProperty(ref _minutesText, value, nameof(MinutesText));
        }

        public string SecondsText
        {
            get => _secondsText;
            set => SetProperty(ref _secondsText, value, nameof(SecondsText));
        }

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
