using System;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace Ferguson.AssetMover.Client.Settings
{
    public class SettingsLoader
    {
        private const string fileName = "ClientSettings.xml";
        private ClientSettings _clientSettings;
        private XDocument _defaultClientSettings;



        public string GetFilePath()
        {
            return Path.Combine(ApplicationFolder.GetPath(), fileName);
        }


        public ClientSettings GetClientSettings()
        {
            return _clientSettings;
        }

        private static XDocument GetResourceDocument(string resourcePath)
        {
            // Default load of settings
            Stream stream = App.GetResourceStream(new Uri(resourcePath, UriKind.Relative)).Stream;
            XmlReader reader = XmlReader.Create(stream);
            XDocument document = XDocument.Load(reader);
            reader.Close();
            stream.Dispose();
            return document;
        }

        private XDocument GetSettingsFromFile(XDocument defaultFile, string fileName)
        {
            if (SettingsFile.Exists(fileName) == false) SettingsFile.Create(defaultFile, fileName);
            return SettingsFile.Load(fileName);
        }

        public void ReplaceSettingsWithFile(string filePath)
        {
            File.Copy(filePath, GetFilePath(), true);
        }

        public void ResolveSettings()
        {
            // System Settings
            // Check if file exists
            _defaultClientSettings = GetResourceDocument("../Settings/Settings.xml");

            // Read system settings
            var filePath = GetFilePath();
            var fileSettings = GetSettingsFromFile(_defaultClientSettings, filePath);
            _clientSettings = ClientSettings.CreateFromXml(fileSettings);
        }
    }


}

