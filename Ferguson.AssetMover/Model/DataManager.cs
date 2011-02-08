using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Ferguson.AssetMover.Client.Settings;

namespace Ferguson.AssetMover.Client.Model
{
    public class DataManager
    {
        public List<ContainerPrefix> Prefixes { get; set; }

        public DataManager()
        {
            Prefixes = new List<ContainerPrefix>();
            CreatePrefixes();
        }


        private void CreatePrefixes()
        {
            ;
            Prefixes = SettingsManager.ClientSettings.CompactKeys;
        }
    }
}
