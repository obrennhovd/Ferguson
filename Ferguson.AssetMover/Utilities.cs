using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferguson.AssetMover.Client.Model;

namespace Ferguson.AssetMover.Client
{
    public class Utilities
    {
        public static MovementType ConvertToMovemenetType(string value)
        {
            return (MovementType)Enum.Parse(typeof(MovementType), value);
        }
    }
}
