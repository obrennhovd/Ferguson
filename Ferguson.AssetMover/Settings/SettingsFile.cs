using System.IO;
using System.Xml.Linq;

namespace Ferguson.AssetMover.Client.Settings
{
    public static class SettingsFile
    {
        private const string settingsFile = @"C:\Ferguson\AssetMover\ClientSettings.xml";

        public static bool Exists()
        {
            return File.Exists(settingsFile);
        }

        public static void Create(XDocument document)
        {
            // Make sure our file structure is in place
            if (Directory.Exists(@"C:\Ferguson") == false)
            {
                Directory.CreateDirectory(@"C:\Ferguson");
            }
            if (Directory.Exists(@"C:\Ferguson\AssetMover") == false)
            {
                Directory.CreateDirectory(@"C:\Ferguson\AssetMover");
            }

            // then save based on xml file
            document.Save(settingsFile);
        }

        public static XDocument Load()
        {
            XDocument document = XDocument.Load(settingsFile);
            return document;
        }
    }
}
