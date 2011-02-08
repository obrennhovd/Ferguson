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

namespace Ferguson.AssetMover.Client.Views
{
    /// <summary>
    /// Interaction logic for PrefixKeyBoardView.xaml
    /// </summary>
    public partial class PrefixKeyBoardView : UserControl, ITextProvider
    {
        DataManager dataManager = new DataManager();
        public PrefixKeyBoardView()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(PrefixKeyBoardView_Loaded);
        }

        void PrefixKeyBoardView_Loaded(object sender, RoutedEventArgs e)
        {
            prefixButtonsControl.ItemsSource = dataManager.Prefixes;
        }

        private void KeyboardButton_Click(object sender, RoutedEventArgs e)
        {
            string keypressed = (sender as FrameworkElement).Tag.ToString();
            // Just single characters should be added to our text.
            if (TextInputOccured != null)
            {
                TextInputOccured(keypressed);
            }
        }

        #region ITextProvider Members

        public event TextInputHandler TextInputOccured;

        #endregion
    }
}
