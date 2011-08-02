using System;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace Ferguson.AssetMover.Client.Settings
{
    public class SettingsLoader
    {
        private readonly ClientSettings _clientSettings;
        private readonly XDocument defaultSettings;

        public SettingsLoader()
        {
             defaultSettings = GetDefaultSettings();
            var fileSettings = GetSettingsFromFile();
            _clientSettings = ClientSettings.CreateFromXml(fileSettings);
        }


        public ClientSettings GetClientSettings()
        {
            return _clientSettings;
        }

        private static XDocument GetDefaultSettings()
        {
            // Default load of settings
            Stream stream = App.GetResourceStream(new Uri("../Settings/Settings.xml", UriKind.Relative)).Stream;
            XmlReader reader = XmlReader.Create(stream);
            XDocument document = XDocument.Load(reader);
            reader.Close();
            stream.Dispose();
            return document;
        }

        private XDocument GetSettingsFromFile()
        {
            if (SettingsFile.Exists() == false) SettingsFile.Create(defaultSettings);
            return SettingsFile.Load();
        }
    }

    
}
