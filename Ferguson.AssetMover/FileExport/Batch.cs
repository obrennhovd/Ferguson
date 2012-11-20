using System;
using Ferguson.AssetMover.Client.Model;
using System.Collections.ObjectModel;

namespace Ferguson.AssetMover.Client.FileExport
{
    public class Batch : BaseEntity
    {

        public Batch(int number)
        {
            AssetMovements = new ObservableCollection<AssetMovement>();
            AssetMovements.CollectionChanged += (sender, args) => AsserIfBatchCanBeTransferred();
            Name = DateTime.Now.ToString("MMddyyyy HH-mm-ss");
        }

        private void AsserIfBatchCanBeTransferred()
        {
            if (AssetMovements.Count > 0 && IsTransferring == false)
            {
                CanTransfer = true;
            }
            else
            {
                CanTransfer = false;
            }
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
                    OnPropertyChanged("HasTransferFailed");
                }
            }
        }

        private bool _isTransferring;
        public bool IsTransferring
        {
            get { return _isTransferring; }
            set
            {
                if (_isTransferring != value)
                {
                    _isTransferring = value;
                    OnPropertyChanged("IsTransferring");
                    AsserIfBatchCanBeTransferred();
                }
            }
        }

        private bool _canTransfer;
        public bool CanTransfer
        {
            get { return _canTransfer; }
            set
            {
                if (_canTransfer != value)
                {
                    _canTransfer = value;
                    OnPropertyChanged("CanTransfer");
                }
            }
        }



    }
}
