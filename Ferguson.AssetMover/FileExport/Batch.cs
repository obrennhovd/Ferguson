using System;
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

        private bool _hasTransferFailed = false;
        public bool HasTransferFailed
        {
            get { return _hasTransferFailed; }
            set
            {
                if (_hasTransferFailed != value)
                {
                    _hasTransferFailed = value;
                    base.OnPropertyChanged("HasTransferFailed");
                }
            }
        }

    }
}
