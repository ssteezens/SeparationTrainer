using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Data.Repositories;
using SeparationTrainer.Models;
using SeparationTrainer.Models.Validation;
using SeparationTrainer.Views;
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
        private ValidatableObject<string> _hoursTextInput = new ValidatableObject<string> { Value = "01" };
        private ValidatableObject<string> _minutesTextInput;
        private ValidatableObject<string> _secondsTextInput;

        public EditActivityViewModel()
        {
            UpdateActivityCommand = new Command(async () => await UpdateActivity());
            CancelCommand = new Command(async () => await Cancel());
            RemoveTagCommand = new Command<TagModel>(RemoveTag);

            HoursTextInput = new ValidatableObject<string>() { Value = "00" };
            HoursTextInput.Validations.Add(new HourTextIsValidRule<string>("Hours must be a number between 0 and 24"));

            MinutesTextInput = new ValidatableObject<string>() { Value = "00" };
            MinutesTextInput.Validations.Add(new MinuteTextIsValidRule<string>("Minutes must be a number between 0 and 59"));

            SecondsTextInput = new ValidatableObject<string>() { Value = "00" };
            SecondsTextInput.Validations.Add(new SecondsTextIsValidRule<string>("Seconds must be a number between 0 and 59"));
        }

        public override async Task LoadData()
        {
            if (ActivityToEditId > 0)
            {
                var activity = await ActivityService.GetAsync(ActivityToEditId);

                ActivityToEdit = activity;

                HoursTextInput.Value = ActivityToEdit.ElapsedTime.Hours.ToString().PadLeft(2, '0') ;
                MinutesTextInput.Value = ActivityToEdit.ElapsedTime.Minutes.ToString().PadLeft(2, '0');
                SecondsTextInput.Value = ActivityToEdit.ElapsedTime.Seconds.ToString().PadLeft(2, '0');
            }
        }

        public int ActivityToEditId { get; set; }

        public ValidatableObject<string> HoursTextInput
        {
            get => _hoursTextInput;
            set => SetProperty(ref _hoursTextInput, value, nameof(HoursTextInput));
        }

        public ValidatableObject<string> MinutesTextInput
        {
            get => _minutesTextInput;
            set => SetProperty(ref _minutesTextInput, value, nameof(MinutesTextInput));
        }

        public ValidatableObject<string> SecondsTextInput
        {
            get => _secondsTextInput;
            set => SetProperty(ref _secondsTextInput, value, nameof(SecondsTextInput));
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

        public Command CancelCommand { get; }

        public Command<TagModel> RemoveTagCommand { get; }

        private void RemoveTag(TagModel tagToRemove)
        {
            var activityTagToRemove = ActivityToEdit.Tags.SingleOrDefault(tag => tag.TagModel == tagToRemove);

            if(activityTagToRemove != null)
            {
                ActivityToEdit.Tags.Remove(activityTagToRemove);
            }

            ActivityToEdit.OnPropertyChanged(nameof(ActivityToEdit.Tags));
        }

        private async Task Cancel()
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            await Shell.Current.GoToAsync($"//{nameof(ViewSessionsPage)}");
        }

        private async Task UpdateActivity()
        {
            try
            {
                var elapsedHours = int.Parse(HoursTextInput.Value);
                var elapsedMinutes = int.Parse(MinutesTextInput.Value);
                var elapsedSeconds = int.Parse(SecondsTextInput.Value);
                var elapsedTime = new TimeSpan(0, elapsedHours, elapsedMinutes, elapsedSeconds);

                ActivityToEdit.ElapsedTime = elapsedTime;

                await ActivityService.UpdateAsync(ActivityToEdit);
            }
            catch (Exception e)
            {
                await DialogService.ShowError("Error", "An unexpected error occurred.", "Ok");
            }

            // send message to notify an activity was edited
            MessagingCenter.Send(this, "EditActivity", ActivityToEdit);

            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            await Shell.Current.GoToAsync($"//{nameof(ViewSessionsPage)}");
        }
    }
}
