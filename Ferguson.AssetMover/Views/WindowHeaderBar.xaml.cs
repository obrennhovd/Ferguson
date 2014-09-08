using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net.NetworkInformation;
using Ferguson.AssetMover.Client.FileExport;
using Ferguson.AssetMover.Client.Model;

namespace Ferguson.AssetMover.Client.Views
{
    /// <summary>
    /// </summary>

    public partial class WindowHeaderBar : Page
    {
        PowerModeChangedEventHandler _powerModeChangeHandler;
        NetworkAvailabilityChangedEventHandler _networkChangedHandler;
        PowerNetworkAware _powerAndNetworkStatus;
        private DispatcherTimer _formTimer;
        private LinearGradientBrush _greyGradientBrush;
        private delegate void UpdateNetworkStatusDelegate(); // <- this delegate is needed for calling of "UpdateNetworkStatus" from dispatcher

        public WindowHeaderBar()
        {
            InitializeComponent();

            // Initially hide the min and close buttons
            MinimizeControl.Visibility = Visibility.Collapsed;
            CloseControl.Visibility = Visibility.Collapsed;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Create the power and network aware class
            _powerAndNetworkStatus = new PowerNetworkAware();

            // Register a delegate for the power related event
            _powerModeChangeHandler = OnPowerModeChange;
            SystemEvents.PowerModeChanged += _powerModeChangeHandler;

            // Register a delegate for the network related event
            _networkChangedHandler = NetworkChange_NetworkAvailabilityChanged;
            NetworkChange.NetworkAvailabilityChanged += _networkChangedHandler;

            // Wire up power awareness
            UpdatePowerStatus();
            // Wire up network status
            UpdateNetworkStatus();

            // Update the time display
            TimeLabel.Content = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();

            // Set the timer to update the time display
            _formTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1d), IsEnabled = true};
            _formTimer.Tick += new EventHandler(timer_Tick);

            // Brush used for mouse-over button background
            _greyGradientBrush = new LinearGradientBrush(Color.FromArgb(155, 55, 55, 55), Color.FromArgb(55, 55, 55, 55), 0.0);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.TimeLabel.Content = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        }

        #region Button Handlers
        void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            // Close the main window and any children
            foreach (Window window in Application.Current.Windows)
            {
                // Close the main window
                window.Close();
            }
        }

        void OnMinButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        void PowerStatusClick(object sender, RoutedEventArgs e)
        {
            Process.Start("rundll32", "shell32,Control_RunDLL powercfg.cpl");
        }
        void NetworkStatusClick(object sender, RoutedEventArgs e)
        {
            Process.Start("rundll32", "shell32,Control_RunDLL ncpa.cpl");
        }
        void TimeStatusClick(object sender, RoutedEventArgs e)
        {
            Process.Start("rundll32", "shell32,Control_RunDLL timedate.cpl");
        }
        void OnHelpButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Show Help File");
        }
        void OnHomeButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Show Home Screen, if one is available");
        }
        void ButtonMouseEnter(object sender, RoutedEventArgs e)
        {
            ContentControl button = (ContentControl)sender;
            button.BorderBrush = Brushes.White;
        }
        void ButtonMouseLeave(object sender, RoutedEventArgs e)
        {
            ContentControl button = (ContentControl)sender;
            button.BorderBrush = Brushes.Black;
        }
        void BorderMouseEnter(object sender, RoutedEventArgs e)
        {
            Border button = (Border)sender;

            button.Background = _greyGradientBrush;
        }
        void BorderMouseLeave(object sender, RoutedEventArgs e)
        {
            Border button = (Border)sender;
            button.Background = null;
        }

        /// <summary>
        /// Toggle between windowed and full-screen mode
        /// to make use of the full abmount of screen real estate
        /// </summary>
        void OnFullScreenClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowStyle != WindowStyle.None)
            {
                // Show the minimize/close buttons only when in full screen
                this.MinimizeControl.Visibility = Visibility.Visible;
                this.CloseControl.Visibility = Visibility.Visible;
                //Set the window style and state to be borderless and maximized
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                Application.Current.MainWindow.WindowStyle = WindowStyle.None;
                // Set the text to the restore symbol (using Martlett font)
                this.FullScreentext.Text = "2";
            }
            else
            {
                // Hide the minimize/close buttons when windowed
                this.MinimizeControl.Visibility = Visibility.Collapsed;
                this.CloseControl.Visibility = Visibility.Collapsed;
                //Set the window style and state to be 'normal' windowed
                Application.Current.MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                // Set the text to the maximize symbol (using Martlett font)
                FullScreentext.Text = "1";
            }
        }
        #endregion

        #region Power and Network Handlers
        /// <summary>
        /// Event handler to respond to NetworkAvailabilityChanged events.
        /// This indicates that the network connection has just changed
        /// between online and offline.
        /// </summary>
        public void NetworkChange_NetworkAvailabilityChanged(object sender, System.Net.NetworkInformation.NetworkAvailabilityEventArgs e)
        {
            // <- Updated call of NetworkStatusUpdate using Dispatcher            
            this.NetworkStatusIndicator.Dispatcher.BeginInvoke(new UpdateNetworkStatusDelegate(UpdateNetworkStatus), DispatcherPriority.Normal, null);
        }


        /// <summary>
        /// Invoked when the network status changes.
        /// Changes icon and tooltip accordingly.
        /// </summary>
        void UpdateNetworkStatus()
        {
            this._powerAndNetworkStatus.EnumerateNetworks();

            ToolTip myTooltip = new ToolTip();
            myTooltip.Content = _powerAndNetworkStatus.currentNetworkLabel;
            this.NetworkStatusIndicator.ToolTip = myTooltip;

            String networkImageName = @"../Images/wirelessgood.ico";

            if (_powerAndNetworkStatus.networkStatus == "Disconnected")
            {
                networkImageName = @"../Images/wirelessnone.ico";
            }
            else if (_powerAndNetworkStatus.networkStatus == "Connectivity Lo")
            {
                networkImageName = @"../Images/wirelesslow.ico";
            }

            // Create source.
            BitmapImage bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            bi.BeginInit();
            bi.UriSource = new Uri(networkImageName, UriKind.RelativeOrAbsolute);
            bi.EndInit();

            this.NetworkStatusIndicator.Source = bi;
        }

        /// <summary>
        /// Event handler for the PowerModeChanged event.
        /// </summary>
        void OnPowerModeChange(object obj, PowerModeChangedEventArgs e)
        {
            // The PowerModeChange event notifies an application of a pending
            // suspend, a resume, or a power state change.  QuerySuspend is
            // no longer supported, and will go away in Vista when applications
            // will have two seconds to suspend and no ability to override the event.
            switch (e.Mode)
            {
                case PowerModes.StatusChange:
                    UpdatePowerStatus();
                    break;
            }
        }

        /// <summary>
        /// Invoked when the power status changes between battery and AC.
        /// Changes icon and tooltip accordingly.
        /// </summary>
        void UpdatePowerStatus()
        {
            // Update the display to reflect the new power state.
            _powerAndNetworkStatus.UpdatePowerInfo();

            // Power icons are: Charging, Full, High, Low, Critical
            string powerIcon;
            string powerImageName = @"../Images/batterygood.ico";
            if (_powerAndNetworkStatus.currentPowerStatus == ManagedPower._ACLineStatus.AC)
            {
                ACPowerStatusIndicator.Visibility = Visibility.Visible;
            }
            else
            {
                ACPowerStatusIndicator.Visibility = Visibility.Hidden;
            }

            // Set the power tooltipe and icon
            powerIcon = _powerAndNetworkStatus.currentPowerLabel;

            if (_powerAndNetworkStatus.currentBatteryPercentage < 10)
            {
                powerImageName = @"../Images/batterycritical.ico";
            }
            else if (_powerAndNetworkStatus.currentBatteryPercentage < 30)
            {
                powerImageName = @"../Images/batterylow.ico";
            }

            if (PowerStatusIndicator != null)
            {
                ToolTip myTooltip = new ToolTip();
                myTooltip.Content = powerIcon;

                PowerStatusIndicator.ToolTip = myTooltip;

                // Create source.
                BitmapImage bi = new BitmapImage();
                // BitmapImage.UriSource must be in a BeginInit/EndInit block.
                bi.BeginInit();
                bi.UriSource = new Uri(powerImageName, UriKind.RelativeOrAbsolute);
                bi.EndInit();

                PowerStatusIndicator.Source = bi;
            }
        }
        #endregion

        private void OpenReportButton_Click(object sender, RoutedEventArgs e)
        {
            var report = ((Button)sender).Content.ToString();

            var backupManager = new BackupManager();
            backupManager.Init();
            ProcessStartInfo procStartInfo;
            if (report.ToUpper() == "INBOUND REPORT")
            {
                procStartInfo = new ProcessStartInfo("notepad.exe", backupManager.InboundReportFile);
            }
            else
            {
                procStartInfo = new ProcessStartInfo("notepad.exe", backupManager.OutboundReportFile);
            }
            var process = new Process();
            process.StartInfo = procStartInfo;
            process.Start();

        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            ContentManager.ChangeView(typeof(SettingsView));
        }
    }
}