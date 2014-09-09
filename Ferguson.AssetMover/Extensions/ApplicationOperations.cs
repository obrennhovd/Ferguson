using System;
using System.Diagnostics;
using System.Windows;

namespace Ferguson.AssetMover.Client.Extensions
{
    public class ApplicationOperations
    {
        public void Restart()
        {
            var Info = new ProcessStartInfo
            {
                Arguments = "/C ping 127.0.0.1 -n 2 && \"" + System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                FileName = "Ferguson.AssetMover.Client.exe"
            };
            Process.Start(Info);
            Application.Current.Shutdown();
        }

        public void OpenFolder(string path)
        {
            //Process.Start(@"c:\temp");
            // opens the folder in explorer
            try
            {
                Process.Start("explorer.exe", path);
            }
            catch (Exception ex)
            {
                throw  new ApplicationException(string.Format("Could not find folder - {0}", path));
            }
            // throws exception
            //Process.Start(@"c:\does_not_exist");
            //// opens explorer, showing some other folder)
            //Process.Start("explorer.exe", @"c:\does_not_exist");
        }

        public void OpenFileInNotepad(string filePath)
        {
            try
            {
                Process.Start("notepad.exe", filePath);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Could not open file - {0}", filePath));
            }
        }
    }

}
