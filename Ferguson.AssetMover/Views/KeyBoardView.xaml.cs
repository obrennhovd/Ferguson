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
using System.Collections.ObjectModel;

namespace Ferguson.AssetMover.Client.Views
{
    /// <summary>
    /// Interaction logic for KeyBoardView.xaml
    /// </summary>
    public partial class KeyBoardView : UserControl, ITextProvider
    {
        public KeyBoardView()
        {
            InitializeComponent();
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
