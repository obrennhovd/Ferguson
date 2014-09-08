using System;
using System.Windows;
using System.Windows.Controls;
using Ferguson.AssetMover.Client.Model;
using Ferguson.AssetMover.Client.FileExport;

namespace Ferguson.AssetMover.Client.Views
{
    /// <summary>
    /// Interaction logic for ScreenView.xaml
    /// </summary>
    public partial class ScreenView : UserControl
    {
        public ScreenView()
        {
            InitializeComponent();
            Loaded += ScreenView_Loaded;
            Transfers.AssetMovementDeleted += TransferManagerAssetMovementDeleted;
        }

        void TransferManagerAssetMovementDeleted(AssetMovement movement)
        {
            // if our the Assetmovement that was deleted is our current, we need to clear and reset.
            if (CurrentAssetMovement.Equals(movement))
            {
                ResetCurrentAssetMovement();
            }
        }

        void ScreenView_Loaded(object sender, RoutedEventArgs e)
        {
            ResetCurrentAssetMovement();
        }

        void ResetCurrentAssetMovement()
        {
            var movement = new AssetMovement()
            {
                ArrivalDate = DateTime.Now
            };
            CurrentAssetMovement = movement;
            this.DataContext = CurrentAssetMovement;
        }

        private AssetMovement _currentAssetMovement;
        public AssetMovement CurrentAssetMovement
        {
            get { return _currentAssetMovement; }
            set
            {
                if (_currentAssetMovement != value)
                {
                    _currentAssetMovement = value;
                    this.DataContext = CurrentAssetMovement;
                    UpdateEnterButtonStatus();
                }
            }
        }

        public void Input(string input)
        {
            CurrentAssetMovement.UnitNumber += input;
            UpdateEnterButtonStatus();
        }


        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentAssetMovement.UnitNumber.Length == 0) return;
            CurrentAssetMovement.UnitNumber = CurrentAssetMovement.UnitNumber.Remove((CurrentAssetMovement.UnitNumber.Length - 1), 1);
        }

        // Commits the current movement to batch and reset the c
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            // Check whether the value was commited by IN or Out button.
            var tag = (sender as Button).Tag;

            CurrentAssetMovement.MovementType = Utilities.ConvertToMovemenetType(tag.ToString());

            if (CurrentAssetMovement.Batch == null)
            {
                CurrentAssetMovement.Batch = Transfers.CurrentBatch;
            }

            ResetCurrentAssetMovement();
            UpdateEnterButtonStatus();
        }

        void UpdateEnterButtonStatus()
        {
            if (InputTextBox.Text.Length >= 4)
            {
                InButton.IsEnabled = true;
                OutButton.IsEnabled = true;
            }
            else
            {
                InButton.IsEnabled = false;
                OutButton.IsEnabled = false;
            }
        }
    }
}
