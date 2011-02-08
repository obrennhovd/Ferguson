using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Ferguson.AssetMover.Views;

namespace Ferguson.AssetMover.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            string innerEx = "";
            if (e.Exception.InnerException != null)
                innerEx = e.Exception.InnerException.Message;

            MessageBoxWindow message = new MessageBoxWindow(MessageType.Error);
            message.SetMessage("An unexpected error occured",e.Exception.Message + Environment.NewLine + Environment.NewLine + "Inner Exception: " + innerEx );
            message.ShowDialog();
            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}
