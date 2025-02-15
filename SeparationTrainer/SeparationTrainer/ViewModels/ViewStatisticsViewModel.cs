using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeparationTrainer.Models;

namespace SeparationTrainer.ViewModels
{
    public class ViewStatisticsViewModel : BaseViewModel
    {
        private string _avgWeeklyActivityTime;
        private TimeSpan _longestActivityTime;
        private string _longestActivityTimeUnderStressThreshold;
        private string _avgTotalActivitiesPerDay;
        private Dictionary<string, TimeSpan> _dayAverages;
        private string _avgStressLevel = string.Empty;
        private string _totalActivities;
        private string _totalDays;
        private Dictionary<TagModel, int> _mostUsedTags = new Dictionary<TagModel, int>();

        public ViewStatisticsViewModel()
        {
            
        }

        public async Task OnAppearing()
        {
            IsBusy = true;

            var activities = await ActivityService.GetAllAsync();

            Activities = activities?.ToList() ?? new List<ActivityModel>();

            SetStatistics();

            IsBusy = false;
        }

        public void SetStatistics()
        {
            if (Activities == null || Activities.Count == 0)
                return;
            
            // longest activity
            LongestActivityTime = Activities.Select(i => i.ElapsedTime).Max();

            // get avg weekly activity time
            GetDayAverages();

            // average stress level
            AvgStressLevel = Math.Round(Activities.Select(i => i.AnxietyLevel).Average(), 2).ToString();

            // most used tags
            GetMostUsedTags();

            // total activities
            TotalActivities = Activities.Count.ToString();

            // total days
            TotalDays = Activities.GroupBy(i => i.Created.Date).Count().ToString();
        }

        private void GetDayAverages()
        {
            var lastTwoWeekActivities = Activities.Where(i => i.Created > DateTime.Now.AddDays(-14));
            var activitiesByDay = lastTwoWeekActivities.GroupBy(i => i.Created.DayOfWeek);
            var dayAverages = new Dictionary<string, TimeSpan>()
            {
                { "Sunday", TimeSpan.Zero },
                { "Monday", TimeSpan.Zero },
                { "Tuesday", TimeSpan.Zero },
                { "Wednesday", TimeSpan.Zero },
                { "Thursday", TimeSpan.Zero },
                { "Friday", TimeSpan.Zero },
                { "Saturday", TimeSpan.Zero }
            };

            foreach (var group in activitiesByDay)
            {
                var totalActivities = group.Count();
                var totalTicks = group.Select(i => i).Sum(i => i.ElapsedTime.Ticks);
                var avgTicks = totalTicks / totalActivities;
                var timeSpanAvg = new TimeSpan(avgTicks);

                dayAverages[group.Key.ToString()] = timeSpanAvg;
                OnPropertyChanged(nameof(DayAverages));
            }

            DayAverages = dayAverages;
        }

        private void GetMostUsedTags()
        {
            var mostUsedTags = new Dictionary<TagModel, int>();
            var tags = Activities.SelectMany(i => i.Tags).Select(i => i.TagModel);
            var topTagGroups = tags
                .GroupBy(tag => tag.Id)
                .OrderByDescending(i => i.Count())
                .Take(5);

            foreach (var tagGroup in topTagGroups)
            {
                mostUsedTags.Add(tagGroup.First(), tagGroup.Count());
                OnPropertyChanged(nameof(mostUsedTags));
            }


            MostUsedTags = mostUsedTags;
        }

        public List<ActivityModel> Activities { get; set; } = new List<ActivityModel>();

        public Dictionary<string, TimeSpan> DayAverages
        {
            get => _dayAverages;
            set => SetProperty(ref _dayAverages, value, nameof(DayAverages));
        }

        public Dictionary<TagModel, int> MostUsedTags
        {
            get => _mostUsedTags;
            set => SetProperty(ref _mostUsedTags, value, nameof(MostUsedTags));
        }

        public string AvgWeeklyActivityTime
        {
            get => _avgWeeklyActivityTime;
            set => SetProperty(ref _avgWeeklyActivityTime, value, nameof(AvgWeeklyActivityTime));
        }

        public TimeSpan LongestActivityTime
        {
            get => _longestActivityTime;
            set => SetProperty(ref _longestActivityTime, value, nameof(LongestActivityTime));
        }

        public string LongestActivityTimeUnderStressThreshold
        {
            get => _longestActivityTimeUnderStressThreshold;
            set => SetProperty(ref _longestActivityTimeUnderStressThreshold, value, nameof(LongestActivityTimeUnderStressThreshold));
        }

        public string AvgTotalActivitiesPerDay
        {
            get => _avgTotalActivitiesPerDay;
            set => SetProperty(ref _avgTotalActivitiesPerDay, value, nameof(AvgTotalActivitiesPerDay));
        }

        public string AvgStressLevel
        {
            get => _avgStressLevel;
            set => SetProperty(ref _avgStressLevel, value, nameof(AvgStressLevel));
        }

        public string TotalActivities
        {
            get => _totalActivities;
            set => SetProperty(ref _totalActivities, value, nameof(TotalActivities));
        }

        public string TotalDays
        {
            get => _totalDays;
            set => SetProperty(ref _totalDays, value, nameof(TotalDays));
        }
    }
}
