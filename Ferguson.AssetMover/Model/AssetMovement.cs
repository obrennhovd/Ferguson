using System;
using System.Text;
using System.Globalization;
using Ferguson.AssetMover.Client.FileExport;

namespace Ferguson.AssetMover.Client.Model
{
    public enum MovementType
    {
        In,
        Out
    }
    public class AssetMovement: BaseEntity
    {

        public AssetMovement()
        {
            _movementStatus = "Off Hire";
            _consignmentLocation = App.ClientSettings.ConsignmentLocation;
        }

        private Batch _batch;
        public Batch Batch
        {
            get { return _batch; }
            set
            {
                if (_batch != value)
                {
                    if (_batch != null && _batch.AssetMovements.Contains(this))
                    {
                        _batch.AssetMovements.Remove(this);
                    }
                    _batch = value;
                    if (_batch != null && !_batch.AssetMovements.Contains(this))
                    {
                        _batch.AssetMovements.Add(this);
                    }
                    OnPropertyChanged("Batch");
                }
            }
        }

        private string _unitNumber = "";
        public string UnitNumber
        {
            get { return _unitNumber; }
            set
            {
                if (_unitNumber != value)
                {
                    _unitNumber = value;
                    OnPropertyChanged("UnitNumber");
                }
            }
        }

        private MovementType _movementType = MovementType.In;
        public MovementType MovementType
        {
            get { return _movementType; }
            set
            {
                if (_movementType != value)
                {
                    _movementType = value;
                    OnPropertyChanged("MovementType");
                }
            }
        }

        private DateTime _arrivalDate;
        public DateTime ArrivalDate
        {
            get { return _arrivalDate; }
            set
            {
                if (_arrivalDate != value)
                {
                    _arrivalDate = value;
                    OnPropertyChanged("ArrivalDate");
                }
            }
        }


        private readonly string _movementStatus;
        public string MovementStatus
        {
            get { return _movementStatus; }
        }


        private readonly string _consignmentLocation;
        public string ConsignmentLocation
        {
            get { return _consignmentLocation; }
        }

        public string GetTransferFormat()
        {
            string separator = ";";

            StringBuilder builder = new StringBuilder();
            builder.Append(UnitNumber);
            builder.Append(separator);
            
            builder.Append(ArrivalDate.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-us")));
            builder.Append(separator);

            builder.Append(MovementStatus);
            builder.Append(separator);

            builder.Append(ConsignmentLocation);
            builder.Append(separator);

            return builder.ToString();
        }
    }
}
