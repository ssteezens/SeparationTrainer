using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using SeparationTrainer.Extensions;
using SeparationTrainer.Models;
using SeparationTrainer.Themes;
using SkiaSharp;

namespace SeparationTrainer.ViewModels
{
    public class ActivityGraphViewModel : BaseViewModel
    {
        private LineChart _chart;
        private DateTime _startDate;
        private DateTime _endDate;
        private List<TagModel> _tags;
        private TagModel _selectedTag;

        public async Task OnAppearing()
        {
            IsBusy = true;

            StartDate = DateTime.Now.AddDays(-30);
            EndDate = DateTime.Now;

            var activities = await ActivityService.GetAllAsync();
            var activityModels = activities.ToList();

            Activities = activityModels;

            Tags = activityModels
                .SelectMany(i => i.Tags)
                .Select(i => i.TagModel)
                .Distinct()
                .ToList();

            SetupChart();

            IsBusy = false;
        }

        public void SetupChart()
        {
            var activitiesInRange = Activities?
                .Where(i => i.Created >= StartDate && i.Created <= EndDate)?
                .OrderBy(i => i.Created)
                .ToList();

            if (activitiesInRange == null)
                return;

            var entries = new List<ChartEntry>();

            var backgroundColor = SKColor.Parse("#1F1B24");
            var onSurfaceColor = SKColor.Parse("#FFFFFF");

            if (AppState.CurrentTheme == Theme.Light)
            {
                backgroundColor = SKColor.Parse("#FFFFFF");
                onSurfaceColor = SKColor.Parse("#262626");
            }

            var labelIndexes = new int[] { 0, activitiesInRange.Count / 4, activitiesInRange.Count / 2, activitiesInRange.Count / 4 * 3, activitiesInRange.Count - 1 };

            foreach (var activity in activitiesInRange)
            {
                var valueLabel = "";

                if (labelIndexes.Contains(activitiesInRange.IndexOf(activity)))
                    valueLabel = activity.ElapsedTime.ToShortForm();

                var entry = new ChartEntry((float)activity.ElapsedTime.TotalMinutes)
                {
                    Label = activity.Created.ToShortDateString(),
                    ValueLabel = valueLabel,
                    Color = SKColor.Parse("#77d065"),
                    ValueLabelColor = onSurfaceColor
                };

                entries.Add(entry);
            }

            var chart = new LineChart
            {
                Entries = entries,
                LabelTextSize = 28,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = backgroundColor
            };

            Chart = chart;
        }

        public LineChart Chart
        {
            get => _chart;
            set => SetProperty(ref _chart, value, nameof(Chart));
        }

        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value, nameof(StartDate), SetupChart);
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value, nameof(EndDate), SetupChart);
        }

        public List<ActivityModel> Activities { get; set; }

        public List<TagModel> Tags
        {
            get => _tags;
            set => SetProperty(ref _tags, value, nameof(Tags));
        }

        public TagModel SelectedTag
        {
            get => _selectedTag;
            set => SetProperty(ref _selectedTag, value, nameof(SelectedTag));
        }
    }
}
