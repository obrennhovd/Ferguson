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
using System.Windows.Shapes;

namespace Ferguson.AssetMover.Client.Views
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        public string ConfirmationText { get; set; }
        public string ConfirmationValue { get; set; }

        public ConfirmationWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(ConfirmationWindow_Loaded);
        }

        void ConfirmationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
