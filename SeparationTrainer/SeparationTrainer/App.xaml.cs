using SeparationTrainer.Views;
using System;
using System.IO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SeparationTrainer.Data;
using SeparationTrainer.Data.Entities;
using SeparationTrainer.Data.Repositories;
using SeparationTrainer.Data.Services;
using SeparationTrainer.Models;
using SeparationTrainer.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ActivityRepository = SeparationTrainer.Data.Repositories.ActivityRepository;

namespace SeparationTrainer
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            
            RegisterServices();

            MainPage = new AppShell();
        }

        public void RegisterServices()
        {
            var mapperInstance = CreateMapper();
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mydb.db3");
            var activityRepository = new ActivityRepository(databasePath);
            var activityTagRepository = new ActivityTagRepository(databasePath);

            // register activity repository
            DependencyService.RegisterSingleton(activityRepository);
            DependencyService.RegisterSingleton(activityTagRepository);
            // register mapper 
            DependencyService.RegisterSingleton(mapperInstance);
            // register dialog service
            DependencyService.Register<IDialogService, DialogService>();
            // register activity service
            DependencyService.Register<IActivityService, ActivityService>();
        }

        public static IMapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Activity, ActivityModel>().ReverseMap();
                    cfg.CreateMap<Session, SessionModel>().ReverseMap();
                });

            return mapperConfiguration.CreateMapper();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
