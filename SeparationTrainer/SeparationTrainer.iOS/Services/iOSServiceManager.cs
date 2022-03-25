using SeparationTrainer.Services.PlatformProcesses;
using Xamarin.Forms;

[assembly: Dependency(typeof(SeparationTrainer.iOS.Services.iOSServiceManager))]
namespace SeparationTrainer.iOS.Services
{
    public class iOSServiceManager : IServiceManager
    {
        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }
    }
}