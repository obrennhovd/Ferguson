using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferguson.AssetMover.Client.Model;
using System.IO;
using System.Xml.Linq;

namespace Ferguson.AssetMover.Client.FileExport
{
    /// <summary>
    /// Public class that maintains a buffer that will be written to disk so data can be retrieved from session to session.
    /// </summary>
    public class MovementBuffer
    {
        private List<AssetMovement> _buffer = new List<AssetMovement>();
        private readonly string _filePath = @"C:\Ferguson\buffer.xml";

        public MovementBuffer()
        {
            _filePath = App.ClientSettings.BufferFilePath;
            Load();
        }

        public List<AssetMovement> GetMovements()
        {
            return _buffer.ToList();
        }

        public List<AssetMovement> GetMovements(MovementType movementType)
        {
            return _buffer.Where(m => m.MovementType == movementType).ToList();
        }

        public void AddElement(AssetMovement movement)
        {
            if (_buffer.Contains(movement)) return;

            _buffer.Add(movement);
            FlushBuffer();
        }

        public void RemoveElement(AssetMovement movement)
        {
            if (!_buffer.Contains(movement)) return;

            _buffer.Remove(movement);
            FlushBuffer();
        }

        public void RemoveElements(IEnumerable<AssetMovement> movements)
        {
            foreach (var movement in movements)
            {
                _buffer.Remove(movement);
            }
            FlushBuffer();
        }

        private void Load()
        {
            // Check if file exists.
            if (!File.Exists(_filePath))
            {
                FlushBuffer();
            }

            XDocument document = XDocument.Load(_filePath);
            var movements = from movement in document.Descendants("AssetMovement")
                            select new AssetMovement
                            {
                                MovementType = Utilities.ConvertToMovemenetType(movement.Element("MovementType").Value),
                                UnitNumber = movement.Element("UnitNumber").Value,
                                ArrivalDate = Convert.ToDateTime(movement.Element("ArrivalDate").Value)
                            };
            _buffer = movements.ToList();
        }

        private void FlushBuffer()
        {
            XDocument saveDocument = new XDocument(new XDeclaration("1.0", Encoding.UTF8.HeaderName, String.Empty), null);
            var root = new XElement("AssetMovements");
            saveDocument.Add(root);

            foreach (var movement in _buffer)
            {
                var element = new XElement("AssetMovement");
                element.Add(new XElement("UnitNumber", movement.UnitNumber));
                element.Add(new XElement("ArrivalDate", movement.ArrivalDate.ToString()));
                element.Add(new XElement("MovementType", movement.MovementType.ToString()));
                root.Add(element);
            }

            saveDocument.Save(_filePath);

        }

       
    }
}
