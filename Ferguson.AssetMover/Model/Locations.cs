using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferguson.AssetMover.Client.Model
{
    public static class Locations
    {
        private static Location _tananger;
        private static Location _aberdeen;
        static Locations()
        {
            _aberdeen = new Location("Aberdeen", "FNAS, Aberdeen");
            _tananger = new Location("Tananger", "FNAS, Tananger");
        }

        public static Location Aberdeen
        {
            get { return _aberdeen; }
        }

        public static Location Tananger
        {
            get { return _tananger; }
        }
    }
}
