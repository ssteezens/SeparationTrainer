using SeparationTrainer.Models;
using SeparationTrainer.Models.Validation;
using SeparationTrainer.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeparationTrainer.ViewModels
{
    public class EditActivityViewModel : BaseViewModel
    {
        private ActivityModel _activityToEdit = new ActivityModel();
        private string _selectedStressLevel = "1";
        private ValidatableObject<string> _hoursTextInput = new ValidatableObject<string> { Value = "01" };
        private ValidatableObject<string> _minutesTextInput;
        private ValidatableObject<string> _secondsTextInput;

        public EditActivityViewModel()
        {
            UpdateActivityCommand = new Command(async () => await UpdateActivity(), () => CanUpdateActivity);
            CancelCommand = new Command(async () => await Cancel());
            RemoveTagCommand = new Command<TagModel>(RemoveTag);
            AddNewTagCommand = new Command(async () => await AddNewTag());
            AddButtonModel = new AddButtonModel()
            {
                DisplayText = "Add New",
                ClickCommand = AddNewTagCommand
            };

            HoursTextInput = new ValidatableObject<string>() { Value = "00" };
            HoursTextInput.Validations.Add(new HourTextIsValidRule<string>("Hours must be a number between 0 and 24"));

            MinutesTextInput = new ValidatableObject<string>() { Value = "00" };
            MinutesTextInput.Validations.Add(new MinuteTextIsValidRule<string>("Minutes must be a number between 0 and 59"));

            SecondsTextInput = new ValidatableObject<string>() { Value = "00" };
            SecondsTextInput.Validations.Add(new SecondsTextIsValidRule<string>("Seconds must be a number between 0 and 59"));
        }

        private bool CanUpdateActivity => HoursTextInput.IsValid && MinutesTextInput.IsValid && SecondsTextInput.IsValid;

        public override async Task LoadData()
        {
            if (ActivityToEditId > 0)
            {
                var activity = await ActivityService.GetAsync(ActivityToEditId);

                ActivityToEdit = activity;
                OnPropertyChanged(nameof(TagCollection));

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

        public ObservableCollection<object> TagCollection
        {
            get
            {
                if (ActivityToEdit.Tags != null)
                    return new ObservableCollection<object>(ActivityToEdit.Tags.Union(new ObservableCollection<object>() { AddButtonModel }));
                else
                    return new ObservableCollection<object>() { AddButtonModel };

            }
        }

        public AddButtonModel AddButtonModel { get; set; }

        public Command UpdateActivityCommand { get; }

        public Command CancelCommand { get; }

        public Command<TagModel> RemoveTagCommand { get; }

        public Command AddNewTagCommand { get; }

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

            ActivityToEdit.Tags.Add(activityTag);
            OnPropertyChanged(nameof(TagCollection));
        }

        private void RemoveTag(TagModel tagToRemove)
        {
            var activityTagToRemove = ActivityToEdit.Tags.SingleOrDefault(tag => tag.TagModel == tagToRemove);

            if(activityTagToRemove != null)
            {
                ActivityToEdit.Tags.Remove(activityTagToRemove);
                OnPropertyChanged(nameof(TagCollection));
            }

            ActivityToEdit.OnPropertyChanged(nameof(ActivityToEdit.Tags));
            OnPropertyChanged(nameof(TagCollection));
        }

        private async Task Cancel()
        {
            var result = await DialogService.ShowAlert("Cancel?", "Are you sure you would like to cancel this activity?");
            if (!result)
                return;

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
