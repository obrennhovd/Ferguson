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
    }

}
