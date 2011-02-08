using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
