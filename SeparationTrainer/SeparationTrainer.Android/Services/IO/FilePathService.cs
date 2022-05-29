using Android;
using Android.Content.PM;
using SeparationTrainer.Exceptions;
using SeparationTrainer.Services.IO;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(SeparationTrainer.Droid.Services.IO.FilePathService))]
namespace SeparationTrainer.Droid.Services.IO
{
    public class FilePathService : IFilePathService
    {
        public async Task CheckPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

            if(status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
                if (status != PermissionStatus.Granted)
                    throw new DeniedPermissionException();
            }
        }

        public async Task<string> GetDownloadsDirectory()
        {
            await CheckPermissions();

            return Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
        }
    }
}
