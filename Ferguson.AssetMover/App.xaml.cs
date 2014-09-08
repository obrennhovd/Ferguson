using System;
using System.Windows;
using System.Windows.Threading;
using Ferguson.AssetMover.Client.Settings;
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
            var settingsLoader = new SettingsLoader();
            settingsLoader.ResolveSettings();
            _clientSettings = settingsLoader.GetClientSettings();
            
        }

        private static ClientSettings _clientSettings;
        public static ClientSettings ClientSettings
        {
            get { return _clientSettings;  }
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
