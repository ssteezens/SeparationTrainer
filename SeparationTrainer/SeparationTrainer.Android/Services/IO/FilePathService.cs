using System.IO;

namespace SeparationTrainer.Droid.Services.IO
{
    public class FilePathService : IFilePathService
    {
        public string GetDownloadsDirectory()
        {
            return Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
        }
    }
}
