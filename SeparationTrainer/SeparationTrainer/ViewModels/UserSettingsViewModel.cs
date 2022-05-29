using SeparationTrainer.Exceptions;
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
            
            try
            {
                var downloadsFolder = await FilePathService.GetDownloadsDirectory();

                var filepath = Path.Combine(downloadsFolder, $"seperation_trainer_data_{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}_{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}.csv");

                ExcelService.CreateCSV(data, filepath);

                NotificationManager.SendNotification("File Created", $"Data has been exported to file: {filepath}", notificationType: NotificationType.Download);
            }
            catch(DeniedPermissionException)
            {
                await DialogService.ShowError("Export Error", "Permissions for creating export file were denied.");
            }
            catch (Exception ex)
            {
                await DialogService.ShowError("Export Error", $"Unkown error when exporting data: {ex.Message}");
            }
        }
    }
}
