using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Ferguson.AssetMover.Client.Model;

namespace Ferguson.AssetMover.Client.Settings
{
    public class ClientSettings
    {
        public ClientSettings()
        {
            // Default settings
            Version = "0";
            ConsignmentLocation = Locations.Tananger.ConsignmentKey;
        }
        public string Version { get; set; }
        public string ConsignmentLocation { get; set; }
        public string LocalDisk { get; set; }
        public string BackupPath { get; set; }
        public string BufferFilePath { get; set; }
        public string ExportPath { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<ContainerPrefix> CompactKeys { get; set; }

        public static ClientSettings CreateFromXml(XDocument document)
        {
            var attributes = document.Element("ClientSettings").Attributes();
            var versionAttribute = attributes.FirstOrDefault(a => a.Name == "version");
            if (versionAttribute != null)
            {
                var version = new ClientSettings
                {
                    Version = document.Element("ClientSettings").Attribute("version").Value,
                    ConsignmentLocation = document.Element("ClientSettings").Element("ConsignmentLocation").Value,
                    BufferFilePath = document.Element("ClientSettings").Element("BufferFilePath").Value,
                    BackupPath = document.Element("ClientSettings").Element("BackupPath").Value,
                    ExportPath = document.Element("ClientSettings").Element("ExportPath").Value,
                    Username = document.Element("ClientSettings").Element("Username").Value,
                    Password = document.Element("ClientSettings").Element("Password").Value,
                    LocalDisk = document.Element("ClientSettings").Element("LocalDisk").Value,
                    CompactKeys = (from compacKey in document.Descendants("CompactKeys").Descendants()
                                   select new ContainerPrefix(compacKey.Value)).OrderBy(c => c.Prefix).
                        ToList()
                };
                return version;
            }
           
            var settingsWithoutVersion = new ClientSettings
                               {
                                   BufferFilePath = document.Element("ClientSettings").Element("BufferFilePath").Value,
                                   BackupPath = document.Element("ClientSettings").Element("BackupPath").Value,
                                   ExportPath = document.Element("ClientSettings").Element("ExportPath").Value,
                                   Username = document.Element("ClientSettings").Element("Username").Value,
                                   Password = document.Element("ClientSettings").Element("Password").Value,
                                   LocalDisk = document.Element("ClientSettings").Element("LocalDisk").Value,
                                   CompactKeys = (from compacKey in document.Descendants("CompactKeys").Descendants()
                                                  select new ContainerPrefix(compacKey.Value)).OrderBy(c => c.Prefix).
                                       ToList()
                               };
            return settingsWithoutVersion;
        }
    }
}
