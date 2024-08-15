using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SearchServer
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        int totalProcesses = 0;

        private void tmrTick_Tick(object sender, EventArgs e)
        {
            if (totalProcesses > 0)
                return;

            StartConversion();
        }

        private void ShowMessage(string message)
        {
            lstMessage.Items.Insert(0, message);
            lstMessage.Items.Insert(0, " ---------------------------------------------------------- ");
        }

        private void StartConversion()
        {
            var inputFolder = new DirectoryInfo(txtInput.Text);

            foreach (var dirDcn in inputFolder.GetDirectories())
            {
                if (dirDcn.GetFiles("*.dcn").Length == 1)
                {
                    var argument = $"\"{dirDcn.FullName}\" \"{txtOutput.Text}\" -d";   // -d flag will delete the dcn once done

                    var myProcess = new DcnProcess
                    {
                        StartInfo = new ProcessStartInfo(txtDcnSrh.Text)
                        {
                            Arguments = argument,
                            CreateNoWindow = false,
                            ErrorDialog = false,
                            UseShellExecute = false,
                            WindowStyle = ProcessWindowStyle.Normal,
                            RedirectStandardError = true                            
                        },
                        EnableRaisingEvents = true,
                        DcnPath = dirDcn.FullName
                    };

                    ShowMessage(dirDcn.Name + " starting at ... " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                    var datetime = DateTime.Now;

                    myProcess.Exited += MyProcess_Exited;

                    totalProcesses++;

                    myProcess.Start();                  

                }
            }
        }

        private void MyProcess_Exited(object sender, EventArgs e)
        {
            totalProcesses--;

            var exitCode = -1;
            var exitedProcess = (sender as DcnProcess);
            var dcnPath = exitedProcess.DcnPath;

            var dirDcn = new DirectoryInfo(dcnPath);

            exitCode = exitedProcess.ExitCode;
            
            if (exitCode != 0)
            {
                File.Move($"\"{dirDcn.FullName}\\{dirDcn.Name}.dcn", $"\"{dirDcn.FullName}\\{dirDcn.Name}.dcn.error");
            }
            else
            {
                Directory.Delete(dirDcn.FullName);
            }
        }

        private void CleanupDCN()
        {
            var inputDir = new DirectoryInfo(txtInput.Text);
            var dcnFiles = inputDir.GetFiles();

            foreach (var fileInfo in dcnFiles)
            {
                var srhFile = $"{txtOutput.Text}\\{fileInfo.Name}.srh";
                if (File.Exists(srhFile))
                {
                    // delete the DCN
                    File.Delete(fileInfo.FullName);
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (File.Exists(txtDcnSrh.Text) && Directory.Exists(txtInput.Text) && Directory.Exists(txtOutput.Text))
            {
                ShowMessage("Started");
                btnStart.Enabled = false;
                tmrTick.Enabled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure?", "Confirm Close", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                tmrTick.Enabled = false;
                Application.Exit();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lstMessage.Items.Clear();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            txtDcnSrh.Text = Convert.ToString(ConfigurationManager.AppSettings["DcnSrhExe"]);
            txtInput.Text = Convert.ToString(ConfigurationManager.AppSettings["DcnPath"]);
            txtOutput.Text = Convert.ToString(ConfigurationManager.AppSettings["SrhPath"]);

        }
    }
}
