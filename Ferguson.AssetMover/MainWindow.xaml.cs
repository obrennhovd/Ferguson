using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ferguson.AssetMover.Client.Model;
using Ferguson.AssetMover.Client.Views;
using Ferguson.AssetMover.Client.FileExport;

namespace Ferguson.AssetMover.Client
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataManager dataManager;

        List<Button> navigationButtons = new List<Button>();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindowLoaded;
            Transfers.CurrentBatchChanged += TransferManagerCurrentBatchChanged;
        }

        void TransferManagerCurrentBatchChanged(Batch newBatch)
        {
            DataContext = Transfers.CurrentBatch;
        }

        void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            navigationButtons.Add(schemaViewButton);
            navigationButtons.Add(prefixViewButton);
            navigationButtons.Add(elementsListViewButton);
            DataContext = Transfers.CurrentBatch;
            dataManager = new DataManager();
            ContentManager.SetShell(this);
            ContentManager.ChangeView<PrefixKeyBoardView>();
        }

        private void ViewButtonClick(object sender, RoutedEventArgs e)
        {
            var viewSelected = (Type)(sender as FrameworkElement).Tag;
            Button selectedButton = navigationButtons.SingleOrDefault(b => b.Equals(sender));
            List<Button> buttonsNotSelected = navigationButtons.Where(b => !(b.Equals(sender))).ToList();

            selectedButton.Style = (Style)Resources["SelectedViewButtonStyle"];
            foreach (var button in buttonsNotSelected)
            {
                button.Style = (Style)Resources["ViewButtonStyle"];
            }

            ContentManager.ChangeView(viewSelected);
        }

    }
}
