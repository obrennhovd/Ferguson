using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferguson.AssetMover.Client.FileExport;
using Ferguson.AssetMover.Client.Model;
using System.Xml.Linq;
using System.Xml;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.IO;

namespace Ferguson.AssetMover.Client.Settings
{
    public class SettingsManager
    {
        static SettingsManager()
        {
            ClientSettings = new ClientSettings();
            // Default load of settings
            
            Stream stream = App.GetResourceStream(new Uri("../FileExport/Settings.xml",UriKind.Relative)).Stream;
            LoadSettings(stream);
        }

        public static ClientSettings ClientSettings { get; set; }

        public static void LoadSettings(Stream resourceStream)
        {
            XmlReader reader = XmlReader.Create(resourceStream);
            XDocument document = XDocument.Load(reader);
            ClientSettings.BufferFilePath = document.Element("ClientSettings").Element("BufferFilePath").Value;
            ClientSettings.BackupPath = document.Element("ClientSettings").Element("BackupPath").Value;
            ClientSettings.ExportPath = document.Element("ClientSettings").Element("ExportPath").Value;
            ClientSettings.Username = document.Element("ClientSettings").Element("Username").Value;
            ClientSettings.Password = document.Element("ClientSettings").Element("Password").Value;
            ClientSettings.LocalDisk = document.Element("ClientSettings").Element("LocalDisk").Value;
            ClientSettings.CompactKeys = (from compacKey in document.Descendants("CompactKeys").Descendants()
                                          select new ContainerPrefix(compacKey.Value)).OrderBy(c => c.Prefix).ToList();
            reader.Close();
            resourceStream.Dispose();
        }

        public static void ExportSettings(string fileName)
        {
            XDocument document = new XDocument(
            new XElement("ClientSettings",
                    new XElement("BackupPath", ClientSettings.BackupPath),
                    new XElement("ExportPath", ClientSettings.ExportPath),
                    new XElement("CompactKeys")
                    )
            );

            var compactKeysNode = document.Element("ClientSettings").Element("CompactKeys");
            foreach (var prefix in ClientSettings.CompactKeys)
            {
                compactKeysNode.Add(new XElement("CompactKey", prefix.Prefix));
            }

            document.Save(fileName);

        }
    }
}
