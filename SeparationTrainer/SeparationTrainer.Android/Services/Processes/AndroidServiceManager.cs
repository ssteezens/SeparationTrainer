using Android.Content;
using SeparationTrainer.Services.PlatformProcesses;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(SeparationTrainer.Droid.Services.Processes.AndroidServiceManager))]
namespace SeparationTrainer.Droid.Services.Processes
{
    public class AndroidServiceManager : IServiceManager
    {
        public void Start()
        {
            var intent = new Intent(AndroidApp.Context, typeof(TimerService));

            AndroidApp.Context.StartForegroundService(intent);
            AndroidApp.Context.StartService(intent);
        }

        public void Stop()
        {
            var intent = new Intent(AndroidApp.Context, typeof(TimerService));

            AndroidApp.Context.StopService(intent);
        }
    }
}
