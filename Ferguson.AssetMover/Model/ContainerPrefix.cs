using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferguson.AssetMover.Client.Model
{
    public class ContainerPrefix
    {
        public string Prefix { get; set; }

        public ContainerPrefix(string prefix)
        {
            this.Prefix = prefix;
        }
    }
}
