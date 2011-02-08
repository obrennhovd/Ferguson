using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferguson.AssetMover.Client.Model;
using System.IO;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.NetworkInformation;
using Ferguson.AssetMover.Client.Views;
using System.Windows.Threading;
using Ferguson.AssetMover.Client.Settings;
using Ferguson.AssetMover.Views;
using Ferguson.AssetMover.Client.Network;
using System.Collections.Specialized;

namespace Ferguson.AssetMover.Client.FileExport
{
    public delegate void CurrentBatchChangedEventHandler(Batch newBatch);
    public delegate void AssetMovementDeletedEventHandler(AssetMovement movement);

    public class TransferManager
    {
        //TODO: Retrieve from settings
        static string exportPath = @"C:\Ferguson\";

        static int counter = 1;
        static Batch currentBatch;
        static ObservableCollection<Batch> transferredBatches = new ObservableCollection<Batch>();
        static ClientSettings clientSettings;
        private static BufferManager buffer = new BufferManager();


        public static event CurrentBatchChangedEventHandler CurrentBatchChanged;
        public static event AssetMovementDeletedEventHandler AssetMovementDeleted;

        static TransferManager()
        {
            exportPath = SettingsManager.ClientSettings.ExportPath;
            clientSettings = SettingsManager.ClientSettings;
            CurrentBatch = new Batch(0);
            LoadBuffer();
        }

        private static void LoadBuffer()
        {
            foreach(var movement in buffer.GetMovements())
            {
                CurrentBatch.AssetMovements.Add(movement);
            }
        }

        public static Batch CurrentBatch
        {
            get { return currentBatch; }
            set
            {
                if (currentBatch != value)
                {
                    if (currentBatch != null)
                    {
                        currentBatch.AssetMovements.CollectionChanged -= new NotifyCollectionChangedEventHandler(AssetMovements_CollectionChanged);
                    }
                    currentBatch = value;
                    currentBatch.AssetMovements.CollectionChanged += new NotifyCollectionChangedEventHandler(AssetMovements_CollectionChanged);
                    if (CurrentBatchChanged != null)
                        CurrentBatchChanged(currentBatch);
                }
            }

        }

        static void AssetMovements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                buffer.AddElement((AssetMovement) e.NewItems[0]);
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                buffer.RemoveElement((AssetMovement)e.OldItems[0]);
            }
        }

        public static void RemoveAssetMovementFromCurrentBatch(AssetMovement movement)
        {
            CurrentBatch.AssetMovements.Remove(movement);
            if (AssetMovementDeleted != null)
                AssetMovementDeleted(movement);
        }

        public static ObservableCollection<Batch> TransferredBatches
        {
            get { return transferredBatches; }
        }

        public static void Transfer()
        {
            string fileName = CurrentBatch.Name + ".csv";

            string localDisk = clientSettings.LocalDisk;
            string fullPath = localDisk + @"\" + fileName;
            
            // create a writer and open the file
            try
            {
                // Map network drive
                NetworkDrive oNetDrive = new NetworkDrive();
                // Remove mapping
                if (Directory.Exists(localDisk))
                {
                    oNetDrive.LocalDrive = localDisk;
                    oNetDrive.Force = true;
                    oNetDrive.ShareName = exportPath;
                    oNetDrive.UnMapDrive();
                }
                // Map network drive
                oNetDrive.LocalDrive = localDisk;
                oNetDrive.ShareName = exportPath;
                oNetDrive.MapDrive(clientSettings.Username, clientSettings.Password);

                TextWriter tw = new StreamWriter(fullPath);
                // write a line of text to the file
                foreach (var movement in CurrentBatch.AssetMovements.Where(m => m.MovementType == MovementType.In))
                {
                    tw.WriteLine(movement.GetTransferFormat());
                }
                // close the stream
                tw.Close();
                tw.Dispose();

                // Log items to backupmanager
                var backup = new BackupManager();
                backup.AddToInboundReport(CurrentBatch, DateTime.Now);
                backup.AddToOutboundReport(CurrentBatch, DateTime.Now);

                // Addjust buffer
                buffer.RemoveElements(CurrentBatch.AssetMovements);

                // Adjust batches
                TransferredBatches.Add(CurrentBatch);
                CurrentBatch = new Batch(counter++);
            }
            catch (System.IO.IOException ex)
            {
                CurrentBatch.HasTransferFailed = true;
                MessageBoxWindow message = new MessageBoxWindow(MessageType.Error);
                message.SetMessage("Error occured when transferring file", ex.Message + "File: " + fullPath);
                message.ShowDialog();
            }
            catch (Exception ex)
            {
                CurrentBatch.HasTransferFailed = true;
                MessageBoxWindow message = new MessageBoxWindow(MessageType.Error);
                message.SetMessage("An unexpected error occured", ex.Message);
                message.ShowDialog();
            }
        }


    }
}
