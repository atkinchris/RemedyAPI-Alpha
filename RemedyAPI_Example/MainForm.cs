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

        private Queries queries;
        private Server remedy;
        private BackgroundWorker bw = new BackgroundWorker();
        private System.Timers.Timer timer = new System.Timers.Timer();
        private string resultsPath;

        public MainForm() {
            InitializeComponent();

            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            startButton.Click += new System.EventHandler(this.Start);
            timer.Interval = 60 * 1000;
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

            remedy = new Server(username, password);
            queries = new Queries();

            var groups = new string[] {
                "Collaboration Services",
                "CS Deploy & Operate",
                "CS Deploy and Operate",
                "End User Devices"
            };
            
            string today = DateTime.Today.ToShortDateString();
            queries.Add("Submitted", new Query(string.Format("(\'{0}\' > \"{1}\") AND (\'{2}\' < \"{3}\")", "Submit Date", today), groups));
            queries.Add("Resolved", new Query(string.Format("(\'{0}\' > \"{1}\") AND (\'{2}\' < \"{3}\")", "Last Resolved Date", today), groups));

            var outstandingQuery = new Query(string.Format("(\'{0}\' < \"{1}\")", "Status", "Resolved"), groups);
            outstandingQuery.users.Add( "Ian Taylor", true );
            queries.Add( "Outstanding", outstandingQuery );
            ToggleRefresh( true );
            RefreshData(null, null);
        }

        private void Stop(object sender, System.EventArgs e) {
            ToggleRefresh( false );
            remedy = null;
        }

        private void ToggleRefresh( bool start ) {

            usernameInput.Enabled = !start;
            passwordInput.Enabled = !start;
            browseButton.Enabled = !start;
            startButton.Text = start ? "Stop" : "Start";
            if ( start ) {
                startButton.Click -= new System.EventHandler( this.Start );
                startButton.Click += new System.EventHandler( this.Stop );
            }
            else {
                startButton.Click -= new System.EventHandler( this.Stop );
                startButton.Click += new System.EventHandler( this.Start );
            }
            timer.Enabled = start;
        }

        private void RefreshData(object source, ElapsedEventArgs e) {
            statusLabel.Text = "Refreshing data...";
            if (bw.IsBusy != true) {
                bw.RunWorkerAsync();
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e) {
            remedy.ExecuteQuery( queries );
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
            var results = queries.GetResultsCount();

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
