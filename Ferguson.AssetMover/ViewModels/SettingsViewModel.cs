using System.Windows;
using System.Windows.Input;
using Ferguson.AssetMover.Client.Extensions;
using Ferguson.AssetMover.Client.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Ferguson.AssetMover.Client.ViewModels
{
    public class SettingsViewModel: ViewModelBase
    {
        public SettingsViewModel()
        {
            FilePath = new SettingsLoader().GetFilePath();
            OpenSettingsFileCommand = new RelayCommand(OpenFile);
        }

        private void OpenFile()
        {
            // Create OpenFileDialog 
            var dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.FileName = "ClientSettings*";
            dlg.Filter = "Xml Files|*.xml;";

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string fullPath = dlg.FileName;
                if (MessageBox.Show("Do you want to override setting with content of selected file? This operation cannot be undone", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var settingsLoader = new SettingsLoader();
                    settingsLoader.ReplaceSettingsWithFile(fullPath);
                    new ApplicationOperations().Restart();
                }
                
            }

            
        }

        public ICommand OpenSettingsFileCommand { get; set; }

        private string _filePath;
        public string FilePath
        {
            get
            {
                return (_filePath);
            }
            set
            {
                if (_filePath == value) return;
                _filePath = value;
                RaisePropertyChanged(() => FilePath);
            }
        }

    }
}
