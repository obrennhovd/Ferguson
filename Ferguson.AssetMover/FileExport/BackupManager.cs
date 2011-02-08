using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferguson.AssetMover.Client.Settings;
using System.IO;
using Ferguson.AssetMover.Client.Model;

namespace Ferguson.AssetMover.Client.FileExport
{
    public class BackupManager
    {
        private static string backupPath = "";
        public string InboundReportFile { get; set; }
        public string OutboundReportFile { get; set; }

        public BackupManager()
        {
            Init();
        }

        public void Init()
        {
            backupPath = SettingsManager.ClientSettings.BackupPath;
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
            StringBuilder builder = new StringBuilder();

            foreach (var movement in batch.AssetMovements.Where(m => m.MovementType == MovementType.In))
            {
                string line = movement.UnitNumber;
                line = line + Space(20 - movement.UnitNumber.Length);
                line += transferTime.ToString();
                line = line + Space(15);
                line += batch.Name.ToString() + ".csv";
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
            
            InboundReportFile = backupPath + inboundName;
            OutboundReportFile = backupPath + outboundName;
        }

        /// <summary>
        /// Method for validating and making sure the backup path exists.
        /// </summary>
        private void ValidateBackupPath()
        {
            if (backupPath == "") new ApplicationException("The backup path is set to empty");

            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }
        }
    }
}
