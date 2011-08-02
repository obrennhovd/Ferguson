using System.Collections.Generic;

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
            Prefixes = App.ClientSettings.CompactKeys;
        }
    }
}
