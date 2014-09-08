using System.IO;
using System.Xml.Linq;

namespace Ferguson.AssetMover.Client.Settings
{
    public static class SettingsFile
    {
        
        public static bool Exists(string fileName)
        {
            return File.Exists(fileName);
        }

        public static void Create(XDocument document, string path)
        {
            // Make sure our file structure is in place
            var directoryPath = Path.GetDirectoryName(path);
            var directories = Directory.GetDirectories(directoryPath);
            foreach (var directory in directories)
            {
                if (Directory.Exists(directory) == false)
                {
                    Directory.CreateDirectory(directory);
                }    
            }

            // then save based on xml file
            document.Save(path);
        }

        public static XDocument Load(string path)
        {
            XDocument document = XDocument.Load(path);
            return document;
        }
    }
}
