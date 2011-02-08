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
using Ferguson.AssetMover.Client.Model;
using Ferguson.AssetMover.Client.Views;
using Ferguson.AssetMover.Client.FileExport;
using Ferguson.AssetMover.Client.Settings;

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
            Loaded += new RoutedEventHandler(MainWindow_Loaded);
            TransferManager.CurrentBatchChanged += new CurrentBatchChangedEventHandler(TransferManager_CurrentBatchChanged);
        }

        void TransferManager_CurrentBatchChanged(Batch newBatch)
        {
            DataContext = TransferManager.CurrentBatch;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            navigationButtons.Add(schemaViewButton);
            navigationButtons.Add(prefixViewButton);
            navigationButtons.Add(elementsListViewButton);
            this.DataContext = TransferManager.CurrentBatch;
            dataManager = new DataManager();
            ContentManager.SetShell(this);
            ContentManager.ChangeView<PrefixKeyBoardView>();
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            Type viewSelected = (Type)(sender as FrameworkElement).Tag;
            Button selectedButton = navigationButtons.Where(b => b.Equals(sender)).SingleOrDefault();
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
