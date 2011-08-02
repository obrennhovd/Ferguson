using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Ferguson.AssetMover.Client.Views;
using Ferguson.AssetMover.Client.Model;
using Ferguson.AssetMover.Client.FileExport;
using System.Reflection;

namespace Ferguson.AssetMover.Client
{
    public class ContentManager
    {
        private static readonly Dictionary<Type, UserControl> ViewCache = new Dictionary<Type, UserControl>();
        private static UserControl _activeView;
        private static ContentControl _activeArea;
        private static MainWindow _activeShell;

        public static ResourceDictionary Resources
        {
            get { return _activeShell.Resources; }
        }

        private static UserControl GetView<T>(Object dataContext) where T : UserControl, new() 
        {
            UserControl control;
            if (ViewCache.Keys.Contains(typeof(T)))
            {
                control = ViewCache[typeof(T)];
            }
            else
            {
                control = new T();
                ViewCache.Add(typeof(T), control);
            }
            control.DataContext = dataContext;
            _activeView = control;
            return control;
        }

        internal static void ChangeView<T>(object dataContext) where T : UserControl, new()
        {
            UserControl control = GetView<T>(dataContext);
            ChangeMainContent(control);
        }

        internal static void ChangeView<T>() where T : UserControl, new()
        {
            ChangeView<T>(null);
        }

        private static void ChangeMainContent(UserControl control)
        {
            if (control is RegisteredElementsView)
                control.DataContext = Transfers.CurrentBatch;

            // Disconnct eventhandler of current view
            if (_activeView != null && _activeView is ITextProvider)
            {
                (_activeView as ITextProvider).TextInputOccured -= _activeShell.Screen.Input;
            }

            // Add eventhandler for textinput field
            if (control is ITextProvider)
            {
                // Show screen
                (control as ITextProvider).TextInputOccured += new TextInputHandler(_activeShell.Screen.Input);
                _activeShell.screenViewRow.Height = new GridLength(2, GridUnitType.Star);
                _activeShell.contentViewRow.Height = new GridLength(4, GridUnitType.Star);
            }
            else // Hide screen
            {
                _activeShell.screenViewRow.Height = new GridLength(0);
                _activeShell.contentViewRow.Height = new GridLength(6, GridUnitType.Star);
            }

            _activeView = control;
            _activeArea.Content = null;
            _activeArea.Content = control;
            
        }

        public static void ChangeScreenInput(AssetMovement movement)
        {
            _activeShell.Screen.CurrentAssetMovement = movement;
        }

        internal static void SetShell(MainWindow shell)
        {
            _activeArea = shell.mainContentControl;
            _activeShell = shell;
        }

        internal static void ChangeView(UserControl viewSelected)
        {
            ChangeMainContent(viewSelected);
        }

        internal static void ChangeView(Type viewSelected)
        {
            var control = (UserControl)Assembly.GetAssembly(viewSelected).CreateInstance(viewSelected.FullName);
            ChangeMainContent(control);
        }
    }
}
