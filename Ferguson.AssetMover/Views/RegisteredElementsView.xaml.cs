using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            TransferManager.CurrentBatchChanged += new CurrentBatchChangedEventHandler(TransferManager_CurrentBatchChanged);
        }

        void TransferManager_CurrentBatchChanged(Batch newBatch)
        {
            DataContext = newBatch;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            TransferManager.Transfer();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            AssetMovement selectedMovement = elementsList.SelectedValue as AssetMovement;
            if (selectedMovement != null)
            {
                ConfirmationWindow window = new ConfirmationWindow();
                window.ConfirmationText = "Do you really want to delete?:";
                window.ConfirmationValue = selectedMovement.UnitNumber;
                if (window.ShowDialog() == true)
                {
                    TransferManager.RemoveAssetMovementFromCurrentBatch(selectedMovement);
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
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
