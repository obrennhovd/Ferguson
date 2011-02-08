using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferguson.AssetMover.Client.Model;
using System.Collections.ObjectModel;

namespace Ferguson.AssetMover.Client.FileExport
{
    public class Batch: BaseEntity
    {

        public Batch(int number)
        {
            AssetMovements = new ObservableCollection<AssetMovement>();
            Name = DateTime.Now.ToString("MMddyyyy HH-mm-ss");
        }

        public ObservableCollection<AssetMovement> AssetMovements { get; private set; }

        private bool hasTransferFailed = false;
        public bool HasTransferFailed
        {
            get { return hasTransferFailed; }
            set
            {
                if (hasTransferFailed != value)
                {
                    hasTransferFailed = value;
                    base.OnPropertyChanged("HasTransferFailed");
                }
            }
        }

    }
}
