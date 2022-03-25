using AutoMapper;
using SeparationTrainer.Data.Entities;
using SeparationTrainer.Data.Repositories;
using SeparationTrainer.Models;
using SeparationTrainer.Services;
using SeparationTrainer.Services.Data;
using System;
using System.IO;
using Xamarin.Forms;
using ActivityRepository = SeparationTrainer.Data.Repositories.ActivityRepository;
using Tag = SeparationTrainer.Data.Entities.Tag;

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
            //var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mydb.db3");
            var databasePath = GetIosPath();
            var activityRepository = new ActivityRepository(databasePath);
            var activityTagRepository = new ActivityTagRepository(databasePath);
            var tagRepository = new TagRepository(databasePath);
            var activityService = new ActivityService(activityRepository, activityTagRepository, tagRepository, mapperInstance);
            var tagService = new TagService(tagRepository, mapperInstance);

            // register activity repository
            DependencyService.RegisterSingleton(activityRepository);
            DependencyService.RegisterSingleton(activityTagRepository);
            DependencyService.RegisterSingleton(tagRepository);
            DependencyService.RegisterSingleton(tagService);
            // register mapper 
            DependencyService.RegisterSingleton(mapperInstance);
            // register dialog service
            DependencyService.Register<IDialogService, DialogService>();
            // register activity service
            DependencyService.RegisterSingleton(activityService);
        }


        public string GetIosPath()
        {
            var dbName = "mydb.db3";
            var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
            var libraryPath = Path.Combine (documentsPath, "..", "Library"); 
            var path = Path.Combine(libraryPath, dbName);

            return path;
        }

        public static IMapper CreateMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Activity, ActivityModel>().ReverseMap();
                    cfg.CreateMap<Tag, TagModel>().ReverseMap();
                    cfg.CreateMap<ActivityTags, ActivityTagModel>().ReverseMap();
                    cfg.CreateMap<Session, SessionModel>().ReverseMap();
                });

            return mapperConfiguration.CreateMapper();
        }

        protected override void OnStart()
        {
            MessagingCenter.Send(this, "OnStart");
        }

        protected override void OnSleep()
        {
            MessagingCenter.Send(this, "OnSleep");
        }

        protected override void OnResume()
        {
            MessagingCenter.Send(this, "OnResume");
        }
    }
}
