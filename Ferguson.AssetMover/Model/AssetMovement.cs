using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
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
            movementStatus = "Off Hire";
            // This property should be read from config
            consignmentLocation = "FNAS, Tananger";
        }

        private Batch batch;
        public Batch Batch
        {
            get { return batch; }
            set
            {
                if (batch != value)
                {
                    if (batch != null && batch.AssetMovements.Contains(this))
                    {
                        batch.AssetMovements.Remove(this);
                    }
                    batch = value;
                    if (batch != null && !batch.AssetMovements.Contains(this))
                    {
                        batch.AssetMovements.Add(this);
                    }
                    base.OnPropertyChanged("Batch");
                }
            }
        }

        private string unitNumber = "";
        public string UnitNumber
        {
            get { return unitNumber; }
            set
            {
                if (unitNumber != value)
                {
                    unitNumber = value;
                    base.OnPropertyChanged("UnitNumber");
                }
            }
        }

        private MovementType movementType = MovementType.In;
        public MovementType MovementType
        {
            get { return movementType; }
            set
            {
                if (movementType != value)
                {
                    movementType = value;
                    base.OnPropertyChanged("MovementType");
                }
            }
        }

        private DateTime arrivalDate;
        public DateTime ArrivalDate
        {
            get { return arrivalDate; }
            set
            {
                if (arrivalDate != value)
                {
                    arrivalDate = value;
                    base.OnPropertyChanged("ArrivalDate");
                }
            }
        }


        private string movementStatus;
        public string MovementStatus
        {
            get { return movementStatus; }
        }


        private string consignmentLocation;
        public string ConsignmentLocation
        {
            get { return consignmentLocation; }
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
