using RemedyAPI;
using System;
using System.ComponentModel;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace RemedyAPI_Example {
    public partial class MainForm : Form {

        private Queries queries;
        private Server remedy;
        private readonly BackgroundWorker bw = new BackgroundWorker();
        private readonly Timer timer = new Timer();
        private string resultsPath;

        public MainForm() {
            InitializeComponent();

            notifyIcon1.Icon = IconGenerator.GetIcon( @"R!" );

            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            startButton.Click += Start;
            timer.Interval = 60 * 1000;
            timer.Elapsed += RefreshData;

            var windowsIdentity = WindowsIdentity.GetCurrent();
            if ( windowsIdentity != null ) {
                var identity = windowsIdentity.Name;
                usernameInput.Text = identity.Substring( identity.IndexOf( @"\", StringComparison.Ordinal ) + 1 );
            }
            outputPathText.Text = @"results.csv";
        }

        private void Start( object sender, EventArgs e ) {

            var username = usernameInput.Text;
            var password = passwordInput.Text;
            resultsPath = outputPathText.Text;

            if ( string.IsNullOrEmpty( username ) ) {
                MessageBox.Show( "Invalid username", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            if ( string.IsNullOrEmpty( password ) ) {
                MessageBox.Show( "Invalid password", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            remedy = new Server( username, password );
            queries = new Queries();

            var groups = new[] {
                "Collaboration Services",
                "CS Deploy & Operate",
                "CS Deploy and Operate",
                "End User Devices"
            };

            string today = DateTime.Today.ToShortDateString();
            queries.Add( "Submitted", new Query( string.Format( "(\'{0}\' > \"{1}\")", "Submit Date", today ), groups ) );
            queries.Add( "Resolved", new Query( string.Format( "(\'{0}\' > \"{1}\")", "Last Resolved Date", today ), groups ) );

            var outstandingQuery = new Query( string.Format( "(\'{0}\' < \"{1}\")", "Status", "Resolved" ), groups );
            outstandingQuery.users.Add( "Ian Taylor", true );
            queries.Add( "Outstanding", outstandingQuery );
            ToggleRefresh( true );
            RefreshData( null, null );
        }

        private void Stop( object sender, EventArgs e ) {
            ToggleRefresh( false );
            remedy = null;
        }

        private void ToggleRefresh( bool start ) {

            usernameInput.Enabled = !start;
            passwordInput.Enabled = !start;
            browseButton.Enabled = !start;
            startButton.Text = start ? "Stop" : "Start";
            if ( start ) {
                startButton.Click -= Start;
                startButton.Click += Stop;
            }
            else {
                startButton.Click -= Stop;
                startButton.Click += Start;
            }
            timer.Enabled = start;
        }

        private void RefreshData( object source, ElapsedEventArgs e ) {
            statusLabel.Text = "Refreshing data...";
            if ( bw.IsBusy != true ) {
                bw.RunWorkerAsync();
            }
        }

        private void bw_DoWork( object sender, DoWorkEventArgs e ) {
            remedy.ExecuteQuery( queries );
        }

        private void bw_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e ) {
            if ( e.Error == null ) {
                UpdateResults();
            }
            else {
                statusLabel.Text = "Error: " + e.Error.Message;
            }
        }

        private void UpdateResults() {
            statusLabel.Text = "Ready.";
            var results = queries.GetResultsCount();

            if ( !File.Exists( resultsPath ) ) {
                var titles = new StringBuilder();
                titles.AppendFormat( "{0}", "Timecode" );
                foreach ( var title in results.Keys ) {
                    titles.AppendFormat( ",{0}", title );
                }
                titles.AppendLine();
                File.WriteAllText( resultsPath, titles.ToString() );
            }

            var output = new StringBuilder();
            output.Append( DateTime.Now );
            foreach ( var result in results.Values ) {
                output.AppendFormat( ",{0}", result );
            }
            using ( StreamWriter sw = File.AppendText( resultsPath ) ) {
                sw.WriteLine( output.ToString() );
            }
        }

        private void browseButton_Click( object sender, EventArgs e ) {
            var dialog = new SaveFileDialog { FileName = "results.csv" };
            if ( dialog.ShowDialog( this ) == DialogResult.OK ) {
                outputPathText.Text = dialog.InitialDirectory + dialog.FileName;
            }
        }

        private void notifyIcon1_DoubleClick( object Sender, EventArgs e ) {
            // Show the form when the user double clicks on the notify icon.
            Show();

            // Set the WindowState to normal if the form is minimized.
            if ( WindowState == FormWindowState.Minimized ) {
                WindowState = FormWindowState.Normal;
            }

            // Activate the form.
            Activate();
        }

        protected override void OnResize( EventArgs e ) {
            if ( WindowState == FormWindowState.Minimized )
                Hide();
            base.OnResize( e );
        }
    }
}
