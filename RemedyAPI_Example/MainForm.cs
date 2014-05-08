using System;
using System.ComponentModel;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using RemedyAPI;
using System.Linq;

namespace RemedyQuery {
    public partial class MainForm : Form {

        private ARConnector remedy;
        private BackgroundWorker bw = new BackgroundWorker();
        private System.Timers.Timer timer = new System.Timers.Timer();
        private string resultsPath;

        public MainForm() {
            InitializeComponent();

            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            startButton.Click += new System.EventHandler(this.Start);
            timer.Interval = 30 * 1000;
            timer.Elapsed += new ElapsedEventHandler(RefreshData);

            var identity = WindowsIdentity.GetCurrent().Name;
            this.usernameInput.Text = identity.Substring(identity.IndexOf(@"\") + 1);
            this.outputPathText.Text = @"results.csv";
        }

        private void Start(object sender, System.EventArgs e) {

            var username = usernameInput.Text;
            var password = passwordInput.Text;
            resultsPath = outputPathText.Text;

            if (username == null || username == string.Empty) {
                MessageBox.Show("Invalid username", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password == null || password == string.Empty) {
                MessageBox.Show("Invalid password", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            usernameInput.Enabled = false;
            passwordInput.Enabled = false;
            startButton.Text = "Stop";
            startButton.Click -= new System.EventHandler(this.Start);
            startButton.Click += new System.EventHandler(this.Stop);

            remedy = new ARConnector(username, password);
            var groups = new string[] {
                "Collaboration Services",
                "CS Deploy & Operate",
                "CS Deploy and Operate",
                "End User Devices"
            };
            remedy.AddGroups(groups);

            string today = DateTime.Today.ToShortDateString();
            remedy.AddQuery("Submitted", string.Format("(\'{0}\' > \"{1}\")", "Submit Date", today));
            remedy.AddQuery("Resolved", string.Format("(\'{0}\' > \"{1}\")", "Last Resolved Date", today));
            remedy.AddQuery("Outstanding", string.Format("(\'{0}\' < \"{1}\")", "Status", "Resolved"));

            timer.Enabled = true;
            RefreshData(null, null);
        }

        private void Stop(object sender, System.EventArgs e) {

            usernameInput.Enabled = true;
            passwordInput.Enabled = true;
            startButton.Text = "Start";
            startButton.Click -= new System.EventHandler(this.Stop);
            startButton.Click += new System.EventHandler(this.Start);
            remedy = null;
            timer.Enabled = false;
        }

        private void RefreshData(object source, ElapsedEventArgs e) {
            statusLabel.Text = "Refreshing data...";
            if (bw.IsBusy != true) {
                bw.RunWorkerAsync();
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e) {
            remedy.ExecuteQueries();
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Error == null) {
                UpdateResults();
            } else {
                statusLabel.Text = "Error: " + e.Error.Message;
            }
        }

        private void UpdateResults() {
            statusLabel.Text = "Ready.";
            var results = remedy.GetResultsCount();

            if (!File.Exists(resultsPath)) {
                var titles = new StringBuilder();
                titles.AppendFormat("{0}", "Timecode");
                foreach (var title in results.Keys) {
                    titles.AppendFormat(",{0}", title);
                }
                titles.AppendLine();
                File.WriteAllText(resultsPath, titles.ToString());
            }

            var output = new StringBuilder();
            output.Append(DateTime.Now.ToString());
            foreach (var result in results.Values) {
                output.AppendFormat(",{0}", result.ToString());
            }
            using (StreamWriter sw = File.AppendText(resultsPath)) {
                sw.WriteLine(output.ToString());
            }
        }

        private void browseButton_Click(object sender, EventArgs e) {
            var dialog = new SaveFileDialog();
            dialog.FileName = "results.csv";
            if (dialog.ShowDialog(this) == DialogResult.OK) {
                outputPathText.Text = dialog.InitialDirectory + dialog.FileName;
            }
        }
    }
}
