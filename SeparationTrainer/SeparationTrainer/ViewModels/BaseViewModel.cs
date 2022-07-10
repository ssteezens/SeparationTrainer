using AutoMapper;
using SeparationTrainer.Data.Repositories;
using SeparationTrainer.Services;
using SeparationTrainer.Services.Data;
using SeparationTrainer.Services.Notifications;
using SeparationTrainer.Services.PlatformProcesses;
using SeparationTrainer.Services.Theme;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeparationTrainer.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            SubscribeToAppLifeCycleEvents();
        }

        public void SubscribeToAppLifeCycleEvents()
        {
            MessagingCenter.Subscribe<App>(this, "OnStart", (sender) =>
            {
                OnStart();
            });
            MessagingCenter.Subscribe<App>(this, "OnSleep", (sender) =>
            {
                OnSleep();
            });
            MessagingCenter.Subscribe<App>(this, "OnResume", (sender) =>
            {
                OnResume();
            });
        }

        protected virtual void OnStart()
        {

        }

        protected virtual void OnSleep()
        {

        }

        protected virtual void OnResume()
        {

        }

        private bool isBusy;

        private string title = string.Empty;
        public IMapper Mapper => DependencyService.Get<IMapper>();

        public ActivityRepository ActivityRepository => DependencyService.Get<ActivityRepository>();

        public TagRepository TagRepository => DependencyService.Get<TagRepository>();

        public ActivityService ActivityService => DependencyService.Get<ActivityService>();

        public TagService TagService => DependencyService.Get<TagService>();

        public IDialogService DialogService => DependencyService.Get<IDialogService>();

        public INotificationManager NotificationManager => DependencyService.Get<INotificationManager>();

        public IServiceManager ServiceManager => DependencyService.Get<IServiceManager>();

        public IThemeService ThemeService => DependencyService.Get<IThemeService>();

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public virtual async Task LoadData()
        {
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}