using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ferguson.AssetMover.Client.FileExport;
using Ferguson.AssetMover.Client.Model;

namespace Ferguson.AssetMover.Client.Views
{
    /// <summary>
    /// Interaction logic for RegisteredElementsView.xaml
    /// </summary>
    public partial class RegisteredElementsView : UserControl
    {
        public RegisteredElementsView()
        {
            InitializeComponent();
            Transfers.CurrentBatchChanged += TransferManagerCurrentBatchChanged;
        }

        void TransferManagerCurrentBatchChanged(Batch newBatch)
        {
            DataContext = newBatch;
        }

        private void SendButtonClick(object sender, RoutedEventArgs e)
        {
            ((Batch) DataContext).IsTransferring = true;
            Transfers.Transfer();
            ((Batch)DataContext).IsTransferring = false;
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            AssetMovement selectedMovement = elementsList.SelectedValue as AssetMovement;
            if (selectedMovement != null)
            {
                ConfirmationWindow window = new ConfirmationWindow();
                window.ConfirmationText = "Do you really want to delete?:";
                window.ConfirmationValue = selectedMovement.UnitNumber;
                if (window.ShowDialog() == true)
                {
                    Transfers.RemoveAssetMovementFromCurrentBatch(selectedMovement);
                }
            }
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            AssetMovement selectedMovement = elementsList.SelectedValue as AssetMovement;
            if (selectedMovement != null)
            {
                ContentManager.ChangeScreenInput(selectedMovement);
                ContentManager.ChangeView<KeyBoardView>();
            }
        }
    }
}
