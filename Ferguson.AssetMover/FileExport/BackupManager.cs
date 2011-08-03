using System;
using System.Linq;
using System.Text;
using System.IO;
using Ferguson.AssetMover.Client.Model;

namespace Ferguson.AssetMover.Client.FileExport
{
    public class BackupManager
    {
        private static string _backupPath = "";
        public string InboundReportFile { get; set; }
        public string OutboundReportFile { get; set; }

        public BackupManager()
        {
            Init();
        }

        public void Init()
        {
            _backupPath = App.ClientSettings.BackupPath;
            ValidateBackupPath();
            ValidateReportFiles();
        }

        public void AddToOutboundReport(Batch batch, DateTime transferTime)
        {
            ValidateReportFiles();

            StreamWriter sw = File.AppendText(OutboundReportFile);
            StringBuilder builder = new StringBuilder();

            foreach (var movement in batch.AssetMovements.Where(m => m.MovementType == MovementType.Out))
            {
                string line = movement.UnitNumber;
                line = line + Space(20 - movement.UnitNumber.Length);
                line += transferTime.ToString();
                line = line + Space(15);
                line += movement.MovementType.ToString();
                builder.AppendLine(line);
            }
            sw.Write(builder.ToString());
            sw.Close();
        }


        public void AddToInboundReport(Batch batch, DateTime transferTime)
        {
            ValidateReportFiles();

            StreamWriter sw = File.AppendText(InboundReportFile);
            var builder = new StringBuilder();

            foreach (var movement in batch.AssetMovements.Where(m => m.MovementType == MovementType.In))
            {
                string line = movement.UnitNumber;
                line = line + Space(20 - movement.UnitNumber.Length);
                line += transferTime.ToString();
                line = line + Space(15);
                line += batch.Name + ".csv";
                builder.AppendLine(line);
            }
            sw.Write(builder.ToString());
            sw.Close();
        }

        private string Space(int length)
        {
            string spacer = "";
            for (int i = 1; i <= length; i++)
            {
                spacer += " ";
            }
            return spacer;
        }

        private void ValidateReportFiles()
        {
            string inboundName = DateTime.Now.ToString("ddMMyyyy") + " - Inbound Total.txt";
            string outboundName = DateTime.Now.ToString("ddMMyyyy") + " - Outbound Total.txt";
            
            InboundReportFile = _backupPath + inboundName;
            OutboundReportFile = _backupPath + outboundName;
        }

        /// <summary>
        /// Method for validating and making sure the backup path exists.
        /// </summary>
        private void ValidateBackupPath()
        {
            if (_backupPath == "") new ApplicationException("The backup path is set to empty");

            if (!Directory.Exists(_backupPath))
            {
                Directory.CreateDirectory(_backupPath);
            }
        }
    }
}
