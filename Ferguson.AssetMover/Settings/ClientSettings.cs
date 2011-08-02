using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Ferguson.AssetMover.Client.Model;

namespace Ferguson.AssetMover.Client.Settings
{
    public class ClientSettings
    {
        public string LocalDisk { get; set; }
        public string BackupPath { get; set; }
        public string BufferFilePath { get; set; }
        public string ExportPath { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<ContainerPrefix> CompactKeys { get; set; }

        public static ClientSettings CreateFromXml(XDocument document)
        {
            var settings = new ClientSettings
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
            return settings;
        }
    }
}
