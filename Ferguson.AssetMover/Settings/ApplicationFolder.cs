using System;
using System.IO;

namespace Ferguson.AssetMover.Client.Settings
{
    public class ApplicationFolder
    {
        public static string GetPath()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var sourceFilePath = Path.Combine(localAppData, "Ferguson AssetMover");
            if (!Directory.Exists(sourceFilePath))
                Directory.CreateDirectory(sourceFilePath);

            return sourceFilePath;
        }

        public static string GetBackupPath()
        {
            var path = Path.Combine(GetPath(), "Backup");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }
    }
}
