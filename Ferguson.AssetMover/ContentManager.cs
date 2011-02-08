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
        private static Dictionary<Type, UserControl> viewCache = new Dictionary<Type, UserControl>();
        private static UserControl activeView;
        private static ContentControl activeArea;
        private static MainWindow activeShell;

        public static ResourceDictionary Resources
        {
            get { return activeShell.Resources; }
        }

        private static UserControl GetView<T>(Object dataContext) where T : new()
        {
            UserControl control;

            if (viewCache.Keys.Contains(typeof(T)))
            {
                control = viewCache[typeof(T)];
            }
            else
            {
                control = new T() as UserControl;
                viewCache.Add(typeof(T), control);
            }

            control.DataContext = dataContext;
            activeView = control;
            return control;
        }

        internal static void ChangeView<T>(object dataContext) where T : new()
        {
            UserControl control = GetView<T>(dataContext);
            ChangeMainContent(control);
        }

        internal static void ChangeView<T>() where T : new()
        {
            ChangeView<T>(null);
        }

        private static void ChangeMainContent(UserControl control)
        {
            if (control is RegisteredElementsView)
                control.DataContext = TransferManager.CurrentBatch;

            // Disconnct eventhandler of current view
            if (activeView != null && activeView is ITextProvider)
            {
                (activeView as ITextProvider).TextInputOccured -= new TextInputHandler(activeShell.Screen.Input);
            }

            // Add eventhandler for textinput field
            if (control is ITextProvider)
            {
                // Show screen
                (control as ITextProvider).TextInputOccured += new TextInputHandler(activeShell.Screen.Input);
                activeShell.screenViewRow.Height = new GridLength(2, GridUnitType.Star);
                activeShell.contentViewRow.Height = new GridLength(4, GridUnitType.Star);
            }
            else // Hide screen
            {
                activeShell.screenViewRow.Height = new GridLength(0);
                activeShell.contentViewRow.Height = new GridLength(6, GridUnitType.Star);
            }

            activeView = control;
            activeArea.Content = null;
            activeArea.Content = control;
            
        }

        public static void ChangeScreenInput(AssetMovement movement)
        {
            activeShell.Screen.CurrentAssetMovement = movement;
        }

        internal static void SetShell(MainWindow shell)
        {
            activeArea = shell.mainContentControl;
            activeShell = shell;
        }

        internal static void ChangeView(UserControl viewSelected)
        {
            ChangeMainContent(viewSelected);
        }

        internal static void ChangeView(Type viewSelected)
        {
            UserControl control = (UserControl)Assembly.GetAssembly(viewSelected).CreateInstance(viewSelected.FullName);
            ChangeMainContent(control);
        }
    }
}
