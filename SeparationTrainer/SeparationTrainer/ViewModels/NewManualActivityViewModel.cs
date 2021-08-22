using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Models;
using SeparationTrainer.Models.Validation;
using SeparationTrainer.Views;
using Xamarin.Forms;

namespace SeparationTrainer.ViewModels
{
    public class NewManualActivityViewModel : BaseViewModel
    {
        private ValidatableObject<string> _hoursTextInput;
        private ValidatableObject<string> _minutesTextInput;
        private ValidatableObject<string> _secondsTextInput;
        private ObservableCollection<ActivityTagModel> _appliedTags = new ObservableCollection<ActivityTagModel>();
        private int _anxietyLevel = 1;

        public NewManualActivityViewModel()
        {
            AddNewTagCommand = new Command(async () => await AddNewTag());
            SaveActivityCommand = new Command(async () => await SaveActivity());
            CancelCommand = new Command(async () => await Cancel());
            RemoveTagCommand = new Command<TagModel>(RemoveTag);

            HoursTextInput = new ValidatableObject<string>() { Value = "00" };
            HoursTextInput.Validations.Add(new HourTextIsValidRule<string>("Hours must be a number between 0 and 24"));

            MinutesTextInput = new ValidatableObject<string>() { Value = "00" };
            MinutesTextInput.Validations.Add(new MinuteTextIsValidRule<string>("Minutes must be a number between 0 and 59"));

            SecondsTextInput = new ValidatableObject<string>() { Value = "00" };
            SecondsTextInput.Validations.Add(new SecondsTextIsValidRule<string>("Seconds must be a number between 0 and 59"));
        }

        #region Properties

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

        public int AnxietyLevel
        {
            get => _anxietyLevel;
            set => SetProperty(ref _anxietyLevel, value, nameof(AnxietyLevel));
        }

        public ObservableCollection<ActivityTagModel> AppliedTags
        {
            get => _appliedTags;
            set => SetProperty(ref _appliedTags, value, nameof(AppliedTags));
        }

        public DateTime SelectedDate { get; set; } = DateTime.Now;

        public string Notes { get; set; }

        #endregion

        #region Commands

        public Command SaveActivityCommand { get; }

        public Command AddNewTagCommand { get; }

        public Command CancelCommand { get; }

        public Command<TagModel> RemoveTagCommand { get; }

        private async Task SaveActivity()
        {
            try
            {
                var elapsedHours = int.Parse(HoursTextInput.Value);
                var elapsedMinutes = int.Parse(MinutesTextInput.Value);
                var elapsedSeconds = int.Parse(SecondsTextInput.Value);
                var elapsedTime = new TimeSpan(0, elapsedHours, elapsedMinutes, elapsedSeconds);

                var activity = new ActivityModel
                {
                    AnxietyLevel = AnxietyLevel,
                    Created = SelectedDate,
                    ElapsedTime = elapsedTime,
                    Notes = Notes,
                    Tags = AppliedTags
                };

                await ActivityService.AddAsync(activity);
            }
            catch (Exception e)
            {
                await DialogService.ShowError("Error", "An unexpected error occurred.", "Ok");
            }

            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

            ResetPage();

            await Shell.Current.GoToAsync($"//{nameof(ViewSessionsPage)}");
        }

        private void ResetPage()
        {
            HoursTextInput.Value = "00";
            MinutesTextInput.Value = "00";
            SecondsTextInput.Value = "00";
            AnxietyLevel = 1;
            Notes = string.Empty;
        }

        private async Task AddNewTag()
        {
            var result = await DialogService.DisplayPrompt("Add Tag",
                "Enter a new tag for this activity",
                "Ok",
                "Cancel",
                "New Tag Here",
                100,
                Keyboard.Text);

            if (result == null)
                return;

            var tagModel = new TagModel { Name = result };
            var returnedTagModel = await TagService.AddAsync(tagModel);

            tagModel.Id = returnedTagModel.Id;

            var activityTag = new ActivityTagModel()
            {
                TagModel = tagModel,
                AppliedOn = DateTime.Now
            };

            AppliedTags.Add(activityTag);
        }

        private async Task Cancel()
        {
            var result = await DialogService.ShowAlert("Cancel?", "Are you sure you would like to cancel this activity?");
            if (!result)
                return;

            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            await Shell.Current.GoToAsync($"//{nameof(ViewSessionsPage)}");
        }
        
        private void RemoveTag(TagModel tagToRemove)
        {
            var activityTagToRemove = AppliedTags.SingleOrDefault(tag => tag.TagModel == tagToRemove);

            if (activityTagToRemove != null)
            {
                AppliedTags.Remove(activityTagToRemove);
            }
        }

        #endregion
    }
}
