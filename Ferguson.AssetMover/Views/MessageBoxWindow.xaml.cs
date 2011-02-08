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

namespace Ferguson.AssetMover.Views
{
    public enum MessageType 
    {
        Error,
        Info
    }

    public class MessageContainer
    {
        public string MessageHeader { get; set; }
        public string MessageText { get; set; }
    }
    /// <summary>
    /// Interaction logic for MessageBoxWindow.xaml
    /// </summary>
    public partial class MessageBoxWindow : Window
    {
        public MessageBoxWindow(MessageType messageType)
        {
            InitializeComponent();
        }

        public void SetMessage(string messageHeader, string messageText)
        {
            DataContext = new MessageContainer() { MessageHeader = messageHeader, MessageText = messageText };
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
