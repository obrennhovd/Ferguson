using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferguson.AssetMover.Client.Model
{
    public class Location
    {
        public Location(string name, string consignmentKey)
        {
            Name = name;
            ConsignmentKey = consignmentKey;
        }

        public string Name { get; set; }
        public string ConsignmentKey { get; set; }
    }

    
}
