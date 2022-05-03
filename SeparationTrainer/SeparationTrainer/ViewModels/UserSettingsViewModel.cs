using SeparationTrainer.Services.IO;
using SeparationTrainer.Services.Notifications;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeparationTrainer.ViewModels
{
    public class UserSettingsViewModel : BaseViewModel
    {
        public UserSettingsViewModel()
        {
            ExportDataCommand = new Command(async () => await ExportData());
        }

        public Command ExportDataCommand { get; set; }

        private async Task ExportData()
        {
            var data = (await ActivityService.GetAllAsync()).ToList();
            var filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"seperation_trainer_data_{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}");

            ExcelService.CreateCSV(data, filepath);

            NotificationManager.SendNotification("File Created", $"Data has been exported to file: {filepath}", notificationType: NotificationType.Download);
        }
    }
}
