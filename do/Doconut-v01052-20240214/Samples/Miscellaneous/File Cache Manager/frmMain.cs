using System;
using System.Windows.Forms;
using System.IO;

namespace FileCacheManager
{
    public partial class frmMain : Form
    {
        private bool isRunning = false;
        private const int SERVICE_INTERVAL_MIN = 60; // service does a check every 60 minutes

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            tmrCheck.Enabled = false;
            Application.Exit();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            tmrCheck.Interval = SERVICE_INTERVAL_MIN * 60 * 1000;  // milliseconds

            if (txtCachePath.Text.Length > 0 && Directory.Exists(txtCachePath.Text))
            {
                if (cacheExpiry.Value >= 10)
                {
                    btnStart.Enabled = false;
                    btnStart.Text = "Running";

                    groupBox1.Enabled = false;
                    
                    tmrCheck.Enabled = true;
                    tmrCheck.Start();

                    CheckCache();
                }
                else
                {
                    ShowLog("Cache clear age should be atleast 10 minutes");
                }
            }
            else
            {
                ShowLog("Invalid cache path");
            }
        }

        private void ShowLog(string message)
        {
            lstLog.Items.Insert(0, message);
            lstLog.Items.Insert(0, "---------------------------------------------");
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            lstLog.Items.Clear();
        }

        private void tmrCheck_Tick(object sender, EventArgs e)
        {
           CheckCache();
        }

        private void CheckCache()
        {
            if (isRunning)
                return;

            try
            {
                isRunning = true;

                ShowLog("Running @ " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString());
               
                ShowLog("Next check at " + DateTime.Now.AddMinutes((double)SERVICE_INTERVAL_MIN).ToShortTimeString());        

                string[] subDirectory = Directory.GetDirectories(txtCachePath.Text);
                var totalDir = subDirectory.Length;

                if (totalDir > 0)
                {
                    ShowLog("There are total " + totalDir + " directories");

                    for (int iCount = 0; iCount < totalDir; iCount++)
                    {
                        var currDir = subDirectory[iCount];

                        var dir = new DirectoryInfo(currDir);

                        if (DateTime.Now.Subtract(dir.CreationTime).TotalMinutes > (double)cacheExpiry.Value)
                        {
                            // Delete all files

                            var filesToDelete = dir.GetFiles();

                            foreach (FileInfo file in filesToDelete)
                            {
                                file.Delete();
                            }

                            ShowLog("Total " + filesToDelete.Length + " files deleted");

                            // Delete the dir itself
                            Directory.Delete(currDir);

                            ShowLog("Deleted " + dir.Name);
                        }
                    }

                    ShowLog("Done"); 
                }
                else
                {
                    ShowLog("Empty directory");
                }
            }
            catch (Exception ex)
            {
                ShowLog(ex.Message);
            }
            finally
            {
                isRunning = false;
            }
        }
    }
}
